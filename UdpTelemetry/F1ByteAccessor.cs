using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Project_RedRacingTelemetry
{
    public class F1BytesAccessor
    {
        private bool isLittleEndian;
        private byte[] buffer;
        public F1BytesAccessor(byte[] buffer, bool isLittleEndian)
        {
            this.buffer = buffer;

            if (isLittleEndian)
            {
                Array.Reverse(this.buffer);
            }
        }

        public int GetInt(long position)
        {
            return Convert.ToInt16(buffer[position]);
        }

        public float GetFloat(long position)
        {
            return Convert.ToSingle(buffer[position]);
        }

        public short GetShort(long position)
        {
            return Convert.ToInt16(buffer[position]);
        }

        public long GetLong(long position)
        {
            return Convert.ToInt64(buffer[position]);
        }

        public double GetDouble(long position)
        {
            return Convert.ToDouble(buffer[position]);
        }

        public byte GetUnsingedByte(long position)
        {
            return GetUnsignedByteValue(buffer[position]);
        }

        private byte GetUnsignedByteValue(Byte b)
        {
            byte v = (byte)(b & 127);
            if (b >> 7 != 0)
            {

                v += 128;
            }
            return v;
        }

        public sbyte GetSignedByte(long position)
        {
            return Convert.ToSByte(buffer[position]);
        }
    }
}
