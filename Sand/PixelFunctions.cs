using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sand
{
    
    public static class Pixels
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static Bytemap byteMap { get; set; }

        public static short[] flowBasic(int index, out bool canMove)
        {
            canMove = false;
            short[] randX = new short[2] { -1, 1 };
            short[] randY = new short[2] { 0, 1 };
            short[] movement = new short[2] { 0, 0 };
            bool canLeft = false;
            bool canRight = false;
            /*
             * CanDo:
             *    [I]
             * [O][1][2] 
             */

            byte[] canDo = byteMap.GetSurroundingBytes(index);
            byte currentByte = byteMap.GetByte(index);
            byte currentMaterial = getMaterial(currentByte);

            if (getMaterial(canDo[1]) < currentMaterial) // check down
            {
                canMove = true;

                //we floating in this bitch?
                if (RandNum(0, 100) > Game.dictFloatFactors[currentMaterial])
                {
                    movement[1] = randY[RandNum(0, 2)];
                }
                else
                {
                    movement[1] = 1;
                }
                if (RandNum(0, 100) > Game.dictFloatFactors[currentMaterial])
                {
                    return movement;
                }
            }

            if (getMaterial(canDo[0]) < currentMaterial) // check L
            {
                movement[0] = -1; movement[1] = 1;
                canLeft = true;
                canMove = true;
            }

            if (getMaterial(canDo[2]) < currentMaterial) // check R
            {
                movement[0] = 1; movement[1] = 1;
                canRight = true;
                canMove = true;
            }

            if (canLeft && canRight)
            {
                movement[0] = randX[RandNum(0, 2)];
            }
            return movement;
        }

        public static short[] flowSmoke(int index, out bool canMove)
        {
            canMove = false;
            short[] randX = new short[2] { -1, 1 };
            short[] randY = new short[2] { 0, 1 };
            short[] movement = new short[2] { 0, 0 };
            bool canLeft = false;
            bool canRight = false;
            /*
             * CanDo:
             *    [I]
             * [O][1][2] 
             */

            byte[] canDo = byteMap.GetSurroundingBytes(index);
            byte currentByte = byteMap.GetByte(index);
            byte currentMaterial = getMaterial(currentByte);

            if (getMaterial(canDo[1]) < currentMaterial) // check down
            {
                canMove = true;

                //we floating in this bitch?
                if (RandNum(0, 100) > Game.dictFloatFactors[currentMaterial])
                {
                    movement[1] = randY[RandNum(0, 2)];
                }
                else
                {
                    movement[1] = 1;
                }
                if (RandNum(0, 100) > Game.dictFloatFactors[currentMaterial])
                {
                    return movement;
                }
            }

            if (getMaterial(canDo[0]) < currentMaterial) // check L
            {
                movement[0] = -1; movement[1] = 1;
                canLeft = true;
                canMove = true;
            }

            if (getMaterial(canDo[2]) < currentMaterial) // check R
            {
                movement[0] = 1; movement[1] = 1;
                canRight = true;
                canMove = true;
            }

            if (canLeft && canRight)
            {
                movement[0] = randX[RandNum(0, 2)];
            }
            return movement;
        }

        public static short[] flowWater(int index, out bool canMove)
        {
            canMove = false;
            short[] randX = new short[2] { -1, 1 };
            short[] randY = new short[2] { 0, 1 };
            short[] movement = new short[2] { 0, 0 };
            bool canLowLeft = false;
            bool canLowRight = false;
            bool canLeft = false;
            bool canRight = false;
            /*
             * CanDo:
             * [3][I][4]
             * [O][1][2] 
             */

            byte[] canDo = byteMap.GetSurroundingBytes(index);
            byte currentByte = byteMap.GetByte(index);
            byte currentMaterial = getMaterial(currentByte);

            if (getMaterial(canDo[1]) < currentMaterial) // check down
            {
                canMove = true;
                movement[1] = 1;
                return movement;
            }

            if (getMaterial(canDo[0]) < currentMaterial) // check LL
            {
                movement[0] = -1; movement[1] = 1;
                canMove = true;
                canLowLeft = true;
            }

            if (getMaterial(canDo[2]) < currentMaterial) // check LR
            {
                movement[0] = 1; movement[1] = 1;
                canMove = true;
                canLowRight = true;
            }

            if (getMaterial(canDo[3]) < currentMaterial) // check L
            {
                movement[0] = -1;
                canMove = true;
                canLeft = true;
            }

            if (getMaterial(canDo[4]) < currentMaterial) // check R
            {
                movement[0] = 1;
                canMove = true;
                canRight = true;
            }

            if ((canLowLeft && canLowRight) || (canLeft && canRight))
            //if (canLeft && canRight)
            {
                movement[0] = randX[RandNum(0, 2)];
            }
            return movement;
        }

        public static short[] flowNone(int index, out bool canMove)
        {
            canMove = false;
            return new short[2] { 0, 0 };
        }

        // 1111 1010 Returns first 4 bits (0000 1010)
        public static byte getMaterial(Byte b)
        {
            byte mask = 15;
            return (byte)(b & mask);
        }

        // 1111 1010 Returns last 4 bits (0000 1111)
        public static byte getBytePixelSettings(Byte b)
        {
            return (b >>= 4);
        }

        public static int RandNum(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
    }
    
    public static class util
    {

        public static Color[] CreateColorScale(Color c)
        {
            float f = 0.00F;
            Color[] scale = new Color[50];
            for (int i = 0; i < 50; i++)
            {
                scale[i] = c.Lerp(Color.White, f);
                f += 0.02F;
            }
            return scale;
        }

        public static Color Lerp(this Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);

            // return the new colour
            return Color.FromArgb(r, g, b);
        }

        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

    }
}
