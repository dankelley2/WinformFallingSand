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

        public void CopyByte(ref DirectBitmap buffer, int i)
        {
            byte mat = getMaterial(Bytes[i]);
            buffer.Bits[i] = Game.dictColors[mat];
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

        public byte[] GetSurroundingBytes(int index)
        {
            return new byte[] { getLowerLeft(index),
                                getDown(index),
                                getLowerRight(index),
                                getLeft(index),
                                getRight(index) };
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

        public byte getDown(int index)
        {
            int b = index + Width;
            if (b > (Width * Height)-1) { return (byte)Game.TYPES.BOUNDS; }
            else { return Bytes[b]; }
        }

        public byte getLowerLeft(int index)
        {
            if (index == 0 || index % Width == 0)
            {
                return (byte)Game.TYPES.BOUNDS;  // out side
            }
            int b = index + Width - 1;
            if (b > (Width * Height) - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out bottom
            }

            else { return Bytes[b]; }
        }

        public byte getLowerRight(int index)
        {
            if (index == 0 || index % Width == Width - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out side
            }
            int b = index + Width + 1;
            if (b > (Width * Height) - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out bottom
            }
            else { return Bytes[b]; }
        }

        public byte getLeft(int index)
        {
            if (index == 0 || index % Width == 0)
            {
                return (byte)Game.TYPES.BOUNDS;  // out side
            }
            int b = index - 1;
            if (b > (Width * Height) - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out bottom
            }
            else { return Bytes[b]; }
        }

        public byte getRight(int index)
        {
            if (index == 0 || index % Width == Width - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out side
            }
            int b = index + 1;
            if (b > (Width * Height) - 1)
            {
                return (byte)Game.TYPES.BOUNDS; // out bottom
            }
            else { return Bytes[b]; }
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
            if (Pixels.getMaterial(Bytes[newIndex]) == (byte)Game.TYPES.BOUNDS) { return; } //Cant swap wall
            Bytes[index] = Bytes[newIndex];
            Bytes[newIndex] = tmpVal;

            CopyByte(ref buffer, index);
            CopyByte(ref buffer, newIndex);
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
