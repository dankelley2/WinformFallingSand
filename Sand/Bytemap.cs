using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sand
{
    public class Bytemap : IDisposable
    {
        public byte[] Bytes { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        protected GCHandle BitsHandle { get; private set; }

        public Bytemap(int width, int height)
        {
            Width = width;
            Height = height;
            Bytes = new byte[width * height];
            BitsHandle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
        }

        public void CopyBytes(ref DirectBitmap buffer)
        {
            for (int i = 0; i < Width * Height; i++)
            {
                byte mat = getMaterial(Bytes[i]);
                buffer.Bits[i] = Game.dictColors[mat];
            }
        }

        public void TintPixel(ref DirectBitmap buffer, int i, double multiplier)
        {
            if (multiplier < 0 || multiplier > 1) return;

            byte mat = getMaterial(Bytes[i]);
            buffer.Bits[i] = Game.dictSleepColors[mat];
        }

        public int[] getRectIndexArray(int x, int y, int width, int height)
        {
            int size = width * height;
            int offset = y * this.Width + x;
            int[] retBytes = new int[size];

            // check if end of rect target is outside the array
            if (offset + size > (this.Width * this.Height))
            {
                throw new ArgumentOutOfRangeException($"The total size of items in the requested rectangle exceeds the length of the array: requested: {size}, available: {Width * Height - offset}");
            }

            int i = 0;
            //iterate through y values
            for (int rowIndex = 0; rowIndex < width; rowIndex++)
            {
                //iterate through x values
                for (int cIndex = 0; cIndex < height; cIndex++)
                {
                    retBytes[i++] = offset++;
                }
                //increment row
                offset += this.Width - width;
            }

            return retBytes;
        }

        public void AddPixelToImageBuffer(ref DirectBitmap buffer, int i)
        {
            byte mat = getMaterial(Bytes[i]);
            buffer.Bits[i] = Game.dictColors[mat];
        }

        public void AddPixelToImageBuffer(ref DirectBitmap buffer, int i, Game.SandType type)
        {
            Bytes[i] = (byte)type;
            buffer.Bits[i] = Game.dictColors[(byte)type];
        }

        public int GetByteIndex(short[] point)
        {
            return point[0] + (point[1] * Width);
        }

        public Byte GetByte(int x, int y)
        {
            int index = x + (y * Width);
            return Bytes[index];

        }

        public Byte GetByte(int index)
        {
            return Bytes[index];
        }

        public Rectangle GetSurroundingRect(int index, int padding, int scale)
        {
            int x = (index % Width);
            int y = (index / Width);
            return new Rectangle((x - padding) * scale,
                                 (y - padding) * scale,
                                (padding << 1) * scale,
                                (padding << 1) * scale);
        }

        public enum Direction
        {
            Down,
            LowerLeft,
            LowerRight,
            Left,
            Right,
            UpperLeft,
            UpperRight,
            Up
        }

        public byte GetElementInDirection(int index, Direction direction)
        {
            int b;

            switch (direction)
            {
                case Direction.Down:
                    b = index + Width;
                    break;
                case Direction.LowerLeft:
                    if (index == 0 || index % Width == 0) return (byte)Game.SandType.BOUNDS;
                    b = index + Width - 1;
                    break;
                case Direction.LowerRight:
                    if (index == 0 || index % Width == Width - 1) return (byte)Game.SandType.BOUNDS;
                    b = index + Width + 1;
                    break;
                case Direction.Left:
                    if (index == 0 || index % Width == 0) return (byte)Game.SandType.BOUNDS;
                    b = index - 1;
                    break;
                case Direction.Right:
                    if (index == 0 || index % Width == Width - 1) return (byte)Game.SandType.BOUNDS;
                    b = index + 1;
                    break;
                case Direction.UpperLeft:
                    if (index == 0 || index < Width || index % Width == 0) return (byte)Game.SandType.BOUNDS;
                    b = index - Width - 1;
                    break;
                case Direction.UpperRight:
                    if (index == 0 || index < Width || index % Width == Width - 1) return (byte)Game.SandType.BOUNDS;
                    b = index - Width + 1;
                    break;
                case Direction.Up:
                    if (index < Width) return (byte)Game.SandType.BOUNDS;
                    b = index - Width;
                    break;
                default:
                    throw new ArgumentException($"Invalid direction: {direction}");
            }

            if (b > (Width * Height) - 1 || b < 0) return (byte)Game.SandType.BOUNDS;

            return Bytes[b];
        }

        public void Fill(byte val)
        {
            for (int i = 0; i < Bytes.Length; i++)
            {
                Bytes[i] = val;
            }
        }

        public void SetByte(int x, int y, byte val)
        {
            int index = x + (y * Width);

            Bytes[index] = val;
        }
        public void SetMaterial(int index, byte mask)
        {
            Bytes[index] >>= 4;
            Bytes[index] <<= 4;
            Bytes[index] |= mask;
        }

        public byte getMaterial(Byte b)
        {
            byte mask = 15;
            return (byte)(b & mask);
        }

        public void bitSwapyCopy(ref DirectBitmap buffer, int index, short[] newPx, out int newIndex)
        {
            byte tmpVal = Bytes[index];
            newIndex = index + newPx[0] + (Width*newPx[1]);
            if (Pixels.getMaterial(Bytes[newIndex]) == (byte)Game.SandType.BOUNDS) { return; } //Cant swap wall
            Bytes[index] = Bytes[newIndex];
            Bytes[newIndex] = tmpVal;

            AddPixelToImageBuffer(ref buffer, index);
            AddPixelToImageBuffer(ref buffer, newIndex);
        }

        public void setMovedFlag(int index, byte val)
        {
            Bytes[index] <<= 1;
            Bytes[index] >>= 1;
            Bytes[index] |= val;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            BitsHandle.Free();
        }
    }

}
