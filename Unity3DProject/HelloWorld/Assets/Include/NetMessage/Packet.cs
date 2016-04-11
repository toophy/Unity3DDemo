using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NetMsg
{
    enum PacketEnum
    {
        MaxWriteLen = 1024,         // 最大写缓冲
        MaxReadLen = 4096,          // 最大读缓冲
        PaketHeaderSize = 4,        // 包头大小
        MsgHeaderSize = 4           // 消息头大小
    }

    class PacketReader : MsgStream
    {
        private int CurrMsgPos;
        private ushort CurrMsgLen;
        private ushort CurrMsgId;
        private ushort Count;

        public PacketReader(byte[] d, ushort count) : base(d)
        {
            CurrMsgId = 0;
            CurrMsgLen = 0;
            CurrMsgPos = 0;
            Count = count;
        }

        public void PreReadMsg(ushort msg_id, ushort msg_len, int start_pos)
        {
            CurrMsgId = msg_id;
            CurrMsgLen = msg_len;
            CurrMsgPos = start_pos;
        }
    }

    class PacketWriter : MsgStream
    {
        private ushort currMsgID;       // 当前消息ID
        private byte msgCount;          // 包内消息总数(包括尾随消息)
        private bool hasPacketHeader;   // 需要写入包头

        public PacketWriter(byte[] d, bool hasHeader = false) : base(d)
        {
            hasPacketHeader = hasHeader;
            Reset();
        }
        public PacketWriter(bool hasHeader) : base(new byte[(int)PacketEnum.MaxWriteLen])
        {
            hasPacketHeader = hasHeader;
            Reset();
        }
        public void Reset()
        {
            Clear();
            currMsgID = 0;
            msgCount = 0;
            if (hasPacketHeader)
            {
                try
                {
                    WriteNull((int)PacketEnum.PaketHeaderSize);
                }
                catch (ReadWriteException e)
                {
                    Console.WriteLine("[E] PacketWriter construct : " + e.Message);
                }
            }
        }
        public bool CanWriteData(int len)
        {
            return (GetPos() + len) < GetMaxLen();
        }
        public byte GetMsgCount()
        {
            return msgCount;
        }
        public void WriteMsgId(ushort id)
        {
            currMsgID = id;
        }
        public void WriteMsgOver()
        {
            ++msgCount;
        }
        public void PacketWriteOver()
        {
            if (hasPacketHeader)
            {
                SeekBegin();
                try
                {
                    WriteUint16((ushort)(GetLen() - (int)PacketEnum.PaketHeaderSize));
                    WriteUint8(0);
                    WriteUint8(msgCount);
                }
                catch (ReadWriteException e)
                {
                    Console.WriteLine("[E] PacketWriter construct : " + e.Message);
                }
                SeekEnd();
            }
        }
    }
}