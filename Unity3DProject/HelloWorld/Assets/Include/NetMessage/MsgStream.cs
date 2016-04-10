using System;
using System.Text;
using System.Runtime.InteropServices;
namespace NetMsg
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct SFloatAndInt32
    {
        [FieldOffset(0)]
        public int a;
        [FieldOffset(0)]
        public float b;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct SFloatAndInt64
        {
        [FieldOffset(0)]
        public long a;
        [FieldOffset(0)]
        public double b;
    }

    class ReadWriteException : ApplicationException
    {
        public ReadWriteException(string message) :
            base(message)
        {
        }
        public ReadWriteException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }

    class MsgStream
    {
        private byte[] Data;
        private int Pos;
        private int CurrLen;
        private int MaxLen;
        public MsgStream(byte[] d)
        {
            Data = d;
            Pos = 0;
            CurrLen = 0;
            MaxLen = d.Length;
        }
        public int GetPos()
        {
            return Pos;
        }
        public int GetLen()
        {
            return CurrLen;
        }
        public void Clear()
        {
            Pos = 0;
            CurrLen = 0;
        }
        public void Seek(int p)
        {
            Pos += p;
            if (Pos < 0) Pos = 0;
            if (Pos > CurrLen) Pos = CurrLen;
        }
        public void SeekBegin()
        {
            Pos = 0;
        }
        public void SeekEnd()
        {
            Pos = CurrLen;
        }
        public byte[] GetData()
        {
            return Data;
        }
        public bool ReadBool()
        {
            if (Pos < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 1; ;
                return Data[o] != 0;
            }
            throw new ReadWriteException("Stream:ReadBool no long");
        }
        public sbyte ReadInt8()
        {
            if (Pos < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 1; ;
                return (sbyte)Data[o];
            }
            throw new ReadWriteException("Stream:ReadInt8 no long");
        }
        public short ReadInt16()
        {
            if (Pos + 1 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 2;
                return (short)(Data[o] | ((Data[o + 1]) << 8));
            }
            throw new ReadWriteException("Stream:ReadInt16 no long");
        }
        public int ReadInt24()
        {
            if (Pos + 2 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 3;
                int n = Data[o] |
                    (Data[o + 1] << 8) |
                    (Data[o + 2] << 16);
                if ((Data[o + 2] & 0x80) == 0x80)
                    n = (int)((uint)n | 0xff000000);
                return n;
            }
            throw new ReadWriteException("Stream:ReadInt24 no long");
        }
        public int ReadInt32()
        {
            if (Pos + 3 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 4;
                return Data[o] |
                    (Data[o + 1] << 8) |
                    (Data[o + 2] << 16) |
                    (Data[o + 3] << 24);
            }
            throw new ReadWriteException("Stream:ReadInt32 no long");
        }
        public long ReadInt40()
        {
            if (Pos + 4 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 5;
                long n = Data[o] |
                    ((long)Data[o + 1] << 8) |
                    ((long)Data[o + 2] << 16) |
                    ((long)Data[o + 3] << 24) |
                    ((long)Data[o + 4] << 32);
                if ((Data[o + 4] & 0x80L) == 0x80L)
                    n = (long)((ulong)n | 0xffffff0000000000);
                return n;
            }
            throw new ReadWriteException("Stream:ReadInt40 no long");
        }
        public long ReadInt48()
        {
            if (Pos + 5 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 6;
                long n = Data[o] |
                    ((long)Data[o + 1] << 8) |
                    ((long)Data[o + 2] << 16) |
                    ((long)Data[o + 3] << 24) |
                    ((long)Data[o + 4] << 32) |
                    ((long)Data[o + 5] << 40);
                if ((Data[o + 5] & 0x80L) == 0x80L)
                    n = (long)((ulong)n | 0xffff000000000000);
                return n;
            }
            throw new ReadWriteException("Stream:ReadInt48 no long");
        }
        public long ReadInt56()
        {
            if (Pos + 6 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 7;
                long n = Data[o] |
                    ((long)Data[o + 1] << 8) |
                    ((long)Data[o + 2] << 16) |
                    ((long)Data[o + 3] << 24) |
                    ((long)Data[o + 4] << 32) |
                    ((long)Data[o + 5] << 40) |
                    ((long)Data[o + 6] << 48);
                if ((Data[o + 6] & 0x80L) == 0x80L)
                    n = (long)((ulong)n | 0xff00000000000000);
                return n;
            }
            throw new ReadWriteException("Stream:ReadInt56 no long");
        }
        public long ReadInt64()
        {
            if (Pos + 7 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 8;
                return (Data[o]) |
                    ((long)Data[o + 1] << 8) |
                    ((long)Data[o + 2] << 16) |
                    ((long)Data[o + 3] << 24) |
                    ((long)Data[o + 4] << 32) |
                    ((long)Data[o + 5] << 40) |
                    ((long)Data[o + 6] << 48) |
                    ((long)Data[o + 7] << 56);
            }
            throw new ReadWriteException("Stream:ReadInt64 no long");
        }
        public byte ReadUint8()
        {
            if (Pos < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 1;
                return Data[o];
            }
            throw new ReadWriteException("Stream:ReadUint8 no long");
        }
        public ushort ReadUint16()
        {
            if (Pos + 1 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 2;
                return (ushort)((Data[o]) | (Data[o + 1] << 8));
            }
            throw new ReadWriteException("Stream:ReadUint16 no long");
        }
        public uint ReadUint24()
        {
            if (Pos + 2 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 3;
                return Data[o] |
                    ((uint)Data[o + 1] << 8) |
                    ((Data[o + 2] & 0x7FU) << 16) |
                    ((Data[o + 2] & 0x80U) << 24);
            }
            throw new ReadWriteException("Stream:ReadUint24 no long");
        }
        public uint ReadUint32()
        {
            if (Pos + 3 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 4;
                return (Data[o]) |
                    ((uint)Data[o + 1] << 8) |
                    ((uint)Data[o + 2] << 16) |
                    ((uint)Data[o + 3] << 24);
            }
            throw new ReadWriteException("Stream:ReadUint32 no long");
        }
        public ulong ReadUint40()
        {
            if (Pos + 4 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 5;
                return Data[o] |
                    ((ulong)Data[o + 1] << 8) |
                    ((ulong)Data[o + 2] << 16) |
                    ((ulong)Data[o + 3] << 24) |
                    ((ulong)Data[o + 4] << 32);
            }
            throw new ReadWriteException("Stream:ReadUint40 no long");
        }
        public ulong ReadUint48()
        {
            if (Pos + 5 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 6;
                return Data[o] |
                    ((ulong)Data[o + 1] << 8) |
                    ((ulong)Data[o + 2] << 16) |
                    ((ulong)Data[o + 3] << 24) |
                    ((ulong)Data[o + 4] << 32) |
                    ((ulong)Data[o + 5] << 40);
            }
            throw new ReadWriteException("Stream:ReadUint48 no long");
        }
        public ulong ReadUint56()
        {
            if (Pos + 6 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 7;
                return Data[o] |
                    ((ulong)Data[o + 1] << 8) |
                    ((ulong)Data[o + 2] << 16) |
                    ((ulong)Data[o + 3] << 24) |
                    ((ulong)Data[o + 4] << 32) |
                    ((ulong)Data[o + 5] << 40) |
                    ((ulong)Data[o + 6] << 48);
            }
            throw new ReadWriteException("Stream:ReadUint56 no long");
        }
        public ulong ReadUint64()
        {
            if (Pos + 7 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 8;
                return Data[o] |
                    ((ulong)Data[o + 1] << 8) |
                    ((ulong)Data[o + 2] << 16) |
                    ((ulong)Data[o + 3] << 24) |
                    ((ulong)Data[o + 4] << 32) |
                    ((ulong)Data[o + 5] << 40) |
                    ((ulong)Data[o + 6] << 48) |
                    ((ulong)Data[o + 7] << 56);
            }
            throw new ReadWriteException("Stream:ReadUint64 no long");
        }
        public float ReadFloat32()
        {
            if (Pos + 3 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 4;
                SFloatAndInt32 dx;
                dx.b = 0.0f;
                dx.a = (Data[o]) |
                    (Data[o + 1] << 8) |
                    (Data[o + 2] << 16) |
                    (Data[o + 3] << 24);
                return dx.b;
            }
            throw new ReadWriteException("Stream:ReadFloat32 no long");
        }
        public double ReadFloat64()
        {
            if (Pos + 7 < MaxLen)
            {
                int o = Pos;
                Pos = Pos + 8;
                SFloatAndInt64 dx;
                dx.b = 0.0;
                dx.a = (Data[o]) |
                    ((long)Data[o + 1] << 8) |
                    ((long)Data[o + 2] << 16) |
                    ((long)Data[o + 3] << 24) |
                    ((long)Data[o + 4] << 32) |
                    ((long)Data[o + 5] << 40) |
                    ((long)Data[o + 6] << 48) |
                    ((long)Data[o + 7] << 56);
                return dx.b;
            }
            throw new ReadWriteException("Stream:ReadFloat64 no long");
        }
        public void ReadData(ref byte[] d, int len)
        {
            if (Pos + len > CurrLen)
            {
                throw new ReadWriteException("ReadData less len");
            }
            else
            {
                Buffer.BlockCopy(Data, Pos, d, 0, len);
                Pos += len;
            }
        }
        public void ReadString(ref string d)
        {
            short dlen = ReadInt16();
            if (dlen > 0)
            {
                byte[] dx = new byte[dlen + 1];
                ReadData(ref dx, dlen);

                d = Encoding.Default.GetString(dx);
            }
        }
        public void WriteBool(bool d)
        {
            if (Pos + 1 < MaxLen)
            {
                if (d) Data[Pos] = 1;
                else Data[Pos] = 0;
                Pos = Pos + 1;
                return;
            }
            throw new ReadWriteException("Stream:WriteBool no long");
        }
        public void WriteInt8(sbyte d)
        {
            if (Pos + 1 < MaxLen)
            {
                Data[Pos] = (byte)d;
                Pos = Pos + 1;
                return;
            }
            throw new ReadWriteException("Stream:WriteInt8 no long");
        }
        public void WriteInt16(short d)
        {
            if (Pos + 2 < MaxLen)
            {
                Data[Pos + 0] = (byte)d;
                Data[Pos + 1] = (byte)(d >> 8);
                Pos = Pos + 2;
                return;
            }
            throw new ReadWriteException("Stream:WriteInt16 no long");
        }
        public void WriteInt24(int d)
        {
            if (Pos + 3 < MaxLen)
            {
                // -2^23 ~ 2^23-1
                if (d >= -8388608 && d <= 8388607)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Pos = Pos + 3;
                    return;
                }
                throw new ReadWriteException("Stream:WriteInt24 out of [-8388608,8388607]");
            }
            throw new ReadWriteException("Stream:WriteInt24 no long");
        }
        public void WriteInt32(int d)
        {
            if (Pos + 4 < MaxLen)
            {
                Data[Pos + 0] = (byte)(d);
                Data[Pos + 1] = (byte)(d >> 8);
                Data[Pos + 2] = (byte)(d >> 16);
                Data[Pos + 3] = (byte)(d >> 24);
                Pos = Pos + 4;
                return;
            }
            throw new ReadWriteException("Stream:WriteInt32 no long");
        }
        public void WriteInt40(long d)
        {
            if (Pos + 5 < MaxLen)
            {
                // -2^39 ~ 2^39-1
                if (d >= -549755813888 && d <= 549755813887)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Pos = Pos + 5;
                    return;
                }
                throw new ReadWriteException("Stream:WriteInt40 out of [-549755813888,549755813887]");
            }
            throw new ReadWriteException("Stream:WriteInt40 no long");
        }
        public void WriteInt48(long d)
        {
            if (Pos + 6 < MaxLen)
            {
                // -2^47 ~ 2^47-1
                if (d >= -140737488355328 && d <= 140737488355327)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Data[Pos + 5] = (byte)(d >> 40);
                    Pos = Pos + 6;
                    return;
                }
                throw new ReadWriteException("Stream:WriteInt48 out of [-140737488355328,140737488355327]");
            }
            throw new ReadWriteException("Stream:WriteInt48 no long");
        }
        public void WriteInt56(long d)
        {
            if (Pos + 7 < MaxLen)
            {
                // -2^55 ~ 2^55-1
                if (d >= -36028797018963968 && d <= 36028797018963967)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Data[Pos + 5] = (byte)(d >> 40);
                    Data[Pos + 6] = (byte)(d >> 48);
                    Pos = Pos + 7;
                    return;
                }
                throw new ReadWriteException("Stream:WriteInt56 out of [-36028797018963968,36028797018963967]");
            }
            throw new ReadWriteException("Stream:WriteInt56 no long");
        }
        public void WriteInt64(long d)
        {
            if (Pos + 8 < MaxLen)
            {
                Data[Pos + 0] = (byte)(d);
                Data[Pos + 1] = (byte)(d >> 8);
                Data[Pos + 2] = (byte)(d >> 16);
                Data[Pos + 3] = (byte)(d >> 24);
                Data[Pos + 4] = (byte)(d >> 32);
                Data[Pos + 5] = (byte)(d >> 40);
                Data[Pos + 6] = (byte)(d >> 48);
                Data[Pos + 7] = (byte)(d >> 56);
                Pos = Pos + 8;
                return;
            }
            throw new ReadWriteException("Stream:WriteInt64 no long");
        }
        public void WriteUint8(byte d)
        {
            if (Pos < MaxLen)
            {
                Data[Pos] = d;
                Pos = Pos + 1;
                return;
            }
            throw new ReadWriteException("Stream:WriteUint8 no long");
        }
        public void WriteUint16(ushort d)
        {
            if (Pos + 1 < MaxLen)
            {
                Data[Pos + 0] = (byte)d;
                Data[Pos + 1] = (byte)(d >> 8);
                Pos = Pos + 2;
                return;
            }
            throw new ReadWriteException("Stream:WriteUint16 no long");
        }
        public void WriteUint24(uint d)
        {
            if (Pos + 2 < MaxLen)
            {
                // 0 ~ 2^24-1
                if (d <= 16777215)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Pos = Pos + 3;
                    return;
                }
                throw new ReadWriteException("Stream:WriteUint24 out of [0,16777215]");
            }
            throw new ReadWriteException("Stream:WriteUint24 no long");
        }
        public void WriteUint32(uint d)
        {
            if (Pos + 3 < MaxLen)
            {
                Data[Pos + 0] = (byte)(d);
                Data[Pos + 1] = (byte)(d >> 8);
                Data[Pos + 2] = (byte)(d >> 16);
                Data[Pos + 3] = (byte)(d >> 24);
                Pos = Pos + 4;
                return;
            }
            throw new ReadWriteException("Stream:WriteUint32 no long");
        }
        public void WriteUint40(ulong d)
        {
            if (Pos + 4 < MaxLen)
            {
                // 0 ~ 2^40-1
                if (d <= 1099511627775)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Pos = Pos + 5;
                    return;
                }
                throw new ReadWriteException("Stream:WriteUint40 out of [0,1099511627775]");
            }
            throw new ReadWriteException("Stream:WriteUint40 no long");
        }
        public void WriteUint48(ulong d)
        {
            if (Pos + 5 < MaxLen)
            {
                // 0 ~ 2^48-1
                if (d <= 1099511627775)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Data[Pos + 5] = (byte)(d >> 40);
                    Pos = Pos + 6;
                    return;
                }
                throw new ReadWriteException("Stream:WriteUint48 out of [0,1099511627775]");
            }
            throw new ReadWriteException("Stream:WriteUint48 no long");
        }
        public void WriteUint56(ulong d)
        {
            if (Pos + 6 < MaxLen)
            {
                // 0 ~ 2^56-1
                if (d <= 72057594037927935)
                {
                    Data[Pos + 0] = (byte)(d);
                    Data[Pos + 1] = (byte)(d >> 8);
                    Data[Pos + 2] = (byte)(d >> 16);
                    Data[Pos + 3] = (byte)(d >> 24);
                    Data[Pos + 4] = (byte)(d >> 32);
                    Data[Pos + 5] = (byte)(d >> 40);
                    Data[Pos + 6] = (byte)(d >> 48);
                    Pos = Pos + 7;
                    return;
                }
                throw new ReadWriteException("Stream:WriteUint56 out of [0,72057594037927935]");
            }
            throw new ReadWriteException("Stream:WriteUint56 no long");
        }
        public void WriteUint64(ulong d)
        {
            if (Pos + 7 < MaxLen)
            {
                Data[Pos + 0] = (byte)(d);
                Data[Pos + 1] = (byte)(d >> 8);
                Data[Pos + 2] = (byte)(d >> 16);
                Data[Pos + 3] = (byte)(d >> 24);
                Data[Pos + 4] = (byte)(d >> 32);
                Data[Pos + 5] = (byte)(d >> 40);
                Data[Pos + 6] = (byte)(d >> 48);
                Data[Pos + 7] = (byte)(d >> 56);
                Pos = Pos + 8;
                return;
            }
            throw new ReadWriteException("Stream:WriteUint64 no long");
        }
        public void WriteFloat32(float d)
        {
            SFloatAndInt32 dx;
            dx.a = 0;
            dx.b = d;
            if (Pos + 3 < MaxLen)
            {
                Data[Pos + 0] = (byte)(dx.a);
                Data[Pos + 1] = (byte)(dx.a >> 8);
                Data[Pos + 2] = (byte)(dx.a >> 16);
                Data[Pos + 3] = (byte)(dx.a >> 24);
                Pos = Pos + 4;
                return;
            }
            throw new ReadWriteException("Stream:WriteFloat32 no long");
        }
        public void WriteFloat64(double d)
        {
            SFloatAndInt64 dx;
            dx.a = 0;
            dx.b = d;
            if (Pos + 7 < MaxLen)
            {
                Data[Pos + 0] = (byte)(dx.a);
                Data[Pos + 1] = (byte)(dx.a >> 8);
                Data[Pos + 2] = (byte)(dx.a >> 16);
                Data[Pos + 3] = (byte)(dx.a >> 24);
                Data[Pos + 4] = (byte)(dx.a >> 32);
                Data[Pos + 5] = (byte)(dx.a >> 40);
                Data[Pos + 6] = (byte)(dx.a >> 48);
                Data[Pos + 7] = (byte)(dx.a >> 56);
                Pos = Pos + 8;
                return;
            }
            throw new ReadWriteException("Stream:WriteFloat64 no long");
        }
        public void WriteData(byte[] d, int len)
        {
            if ((Pos + len) < MaxLen)
            {
                Buffer.BlockCopy(d, 0, Data, Pos, len);
                Pos += len;
                if (Pos > CurrLen)
                    CurrLen = Pos;
                return;
            }
            throw new ReadWriteException("Stream:WriteData no long");
        }
        public void WriteString(ref string d)
        {
            byte[] dx = Encoding.Default.GetBytes(d);
            WriteInt16((short)dx.Length);
            if (dx.Length > 0)
            {
                if (Pos + dx.Length < MaxLen)
                {
                    Buffer.BlockCopy(dx, 0, Data, Pos, dx.Length);

                    Pos += dx.Length;
                    if (Pos > CurrLen)
                        CurrLen = Pos;
                }
                else
                {
                    Pos = Pos -2;
                    throw new ReadWriteException("Stream:WriteString no long");
                }
            }
        }
    }
}