using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sand
{
    /// <summary>
    /// Base Game class. Initialize Bytemap and create render pipeline to form
    /// </summary>
    public partial class Game
    {
        public delegate short[] FlowDelegate(int index, out bool canMove);

        public enum SandType{VOID=0, FLAME=1, EMBER=2, WATER=3, SAND=4, BOUNDS=15}

        public static readonly Dictionary<int, int> dictFloatFactors =
            new Dictionary<int, int> { {(int)SandType.VOID  ,  0},
                                       {(int)SandType.FLAME , 75},
                                       {(int)SandType.EMBER , 25},
                                       {(int)SandType.SAND  , 10},
                                       {(int)SandType.WATER , 10},
                                       {(int)SandType.BOUNDS,  0}
            };

        public static readonly Dictionary<int, int> dictColors =
            new Dictionary<int, int> { {(int)SandType.VOID  , Color.Black.ToArgb()},
                                       {(int)SandType.FLAME , Color.OrangeRed.ToArgb()},
                                       {(int)SandType.EMBER , Color.Red.ToArgb()},
                                       {(int)SandType.SAND  , Color.Tan.ToArgb()},
                                       {(int)SandType.WATER , Color.Blue.ToArgb() },
                                       {(int)SandType.BOUNDS, Color.DarkSlateGray.ToArgb()}
            };

        public static readonly Dictionary<int, int> dictSleepColors =
            new Dictionary<int, int> { {(int)SandType.VOID  , Color.Black.ToArgb()},
                                       {(int)SandType.FLAME , Color.DarkOrange.ToArgb()},
                                       {(int)SandType.EMBER , Color.DarkRed.ToArgb()},
                                       {(int)SandType.SAND  , Color.SandyBrown.ToArgb()},
                                       {(int)SandType.WATER , Color.DarkBlue.ToArgb() },
                                       {(int)SandType.BOUNDS, Color.DarkSlateGray.ToArgb()}
            };

        public static readonly Dictionary<int, FlowDelegate> dictMvmtFnc =
            new Dictionary<int, FlowDelegate> {
                                       {(int)SandType.VOID ,  Pixels.flowNone},
                                       {(int)SandType.FLAME , Pixels.flowBasic},
                                       {(int)SandType.EMBER , Pixels.flowBasic},
                                       {(int)SandType.SAND  , Pixels.flowBasic},
                                       {(int)SandType.WATER , Pixels.flowWater},
                                       {(int)SandType.BOUNDS, Pixels.flowNone}
            };

        private DirectBitmap bmpBuffer;
        private Bytemap bytemap;
        private PictureBox canvas;
        private int scale;
        private short[] mvmtDelta = new short[2];
        private int gridWidth;

        private int[,] Zones;
        public int ActiveZones = 0;
        private int numOfZones;
        private int zoneWidth;

        public Game(ref PictureBoxPixels canvas, int width)
        {
            this.gridWidth = width;
            this.canvas = canvas;
            this.scale = canvas.Width / width;
            bmpBuffer = new DirectBitmap(width, width);
            bytemap = new Bytemap(width, width);
            bytemap.Fill((byte)SandType.VOID); // Fill with Void
            bytemap.CopyBytes(ref bmpBuffer); // Copy to Buffer
            Pixels.byteMap = bytemap;
            canvas.Image = bmpBuffer.Bitmap;
            canvas.SizeMode = PictureBoxSizeMode.StretchImage;

            var zones = 32;

            // Width of each zone, in pixels
            zoneWidth = (width) / zones;

            // Number of zones on each axis
            numOfZones = zones;
            Zones = new int[zones, zones];

            canvas.Invalidate();
        }

        public void RunGame()
        {
            int originalX, originalY;

            // for debugging
            ActiveZones = 0;

            for (int zoneY = numOfZones - 1; zoneY >= 0; zoneY--)  // Loop through zones vertically in reverse
            {
                for (int zoneX = 0; zoneX < numOfZones; zoneX++)  // Loop through zones horizontally
                {
                    // Check for active pixels in zone
                    if (Zones[zoneX, zoneY] == 0) continue;

                    // Increment active zones
                    ActiveZones++;
                    var ActivePixelsInZone = 0;

                    // Reset active pixel count
                    Zones[zoneX, zoneY] = 0;

                    for (int pixelY = zoneWidth - 1; pixelY >= 0; pixelY--)  // Loop through pixels within a zone vertically in reverse
                    {
                        // Run LTR
                        for (int pixelX = 0; pixelX < zoneWidth; pixelX++)  // Loop through pixels within a zone horizontally
                        {
                            // Calculate the original pixel index in the 1D array
                            originalX = zoneX * zoneWidth + pixelX;
                            originalY = zoneY * zoneWidth + pixelY;
                            int originalIndex = originalY * gridWidth + originalX;

                            // Now you can access the pixel
                            if (movePixelsByByteIndex(originalIndex))
                            {
                                ActivateNeighboringZones(zoneX, zoneY);
                                ActivePixelsInZone++;
                            }
                            else
                            {
                                bytemap.TintPixel(ref bmpBuffer, originalIndex, 0.5);
                            }
                        }

                        // shift down
                        pixelY--;

                        // Run RTL
                        for (int pixelX = zoneWidth - 1; pixelX >= 0; pixelX--)  // Loop through pixels within a zone horizontally
                        {
                            // Calculate the original pixel index in the 1D array
                            originalX = zoneX * zoneWidth + pixelX;
                            originalY = zoneY * zoneWidth + pixelY;
                            int originalIndex = originalY * gridWidth + originalX;

                            // Now you can access the pixel
                            if (movePixelsByByteIndex(originalIndex))
                            {
                                ActivateNeighboringZones(zoneX, zoneY);
                                ActivePixelsInZone++;
                            }
                            else
                            {
                                bytemap.TintPixel(ref bmpBuffer, originalIndex, 0.5);
                            }
                        }
                    }

                    // For Debugging, count if nothing in the whole zone was moved
                    if (ActivePixelsInZone == 0)
                    {

                    }

                }
            }

        }

        private void ActivateNeighboringZones(int zoneX, int zoneY)
        {
            // Iterate over all 9 surrounding pixels; which is every possible movement vector, mark those zones as active
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int neighbourX = zoneX + dx;
                    int neighbourY = zoneY + dy;

                    // Skip out-of-bounds pixels
                    if (neighbourX < 0 || neighbourX >= numOfZones || neighbourY < 0 || neighbourY >= numOfZones) continue;

                    // Increment their zone as well
                    Zones[neighbourX, neighbourY]++;
                }
            }
        }

        public void Render()
        {
            canvas.Invalidate();
            canvas.Update();
        }

        private bool movePixelsByByteIndex(int byteIndex)
        {
            byte BytePixel = bytemap.GetByte(byteIndex);

            //Get Material Type
            Byte Material = Pixels.getMaterial(BytePixel); // shift to return first four bits
            Byte Settings = Pixels.getBytePixelSettings(BytePixel); // shift to return first four bits

            // If not Void material
            if (Material == (byte)SandType.VOID) return false;

            // i++;
            mvmtDelta = dictMvmtFnc[(bytemap.getMaterial(bytemap.Bytes[byteIndex]))](byteIndex, out bool canMove);
            if (Material == (byte)SandType.BOUNDS)
            {
                bytemap.AddPixelToImageBuffer(ref bmpBuffer, byteIndex);
            }

            // return if stuck
            if (!canMove) return false;

            //add moved flag for this run
            bytemap.bitSwapyCopy(ref bmpBuffer, byteIndex, mvmtDelta, out int newIndex);

            return true;
        }

        private void AddPixelsOfType(Point location, byte type)
        {
            location.X = Math.Max(Math.Min(location.X, gridWidth - 1), 0);
            location.Y = Math.Max(Math.Min(location.Y, gridWidth - 1), 0);


            foreach (int i in bytemap.getRectIndexArray(50, 50, 50, 50))
            {
                bytemap.AddPixelToImageBuffer(ref bmpBuffer, i, SandType.FLAME);
            }

            //If pixel is not used 
            if (bytemap.GetByte(location.X, location.Y) == (byte)SandType.VOID)
            {
                // update active zone with pixel
                int sx = location.X / zoneWidth;
                int sy = location.Y / zoneWidth;
                Zones[sx, sy]++;

                bytemap.SetByte(location.X, location.Y, type);
            }

        }
    }
}
