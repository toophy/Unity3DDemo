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

        public PacketReader(byte[] d, int len, ushort count) : base(d,len)
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
        private int lastMsgBeginPos;    // 最后一个消息的开启位置
        private int lastMsgEndPos;      // 最好一个消息的结束位置

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
            lastMsgBeginPos = 0;
            lastMsgEndPos = 0;
            if (hasPacketHeader)
            {
                try
                {
                    WriteNull((int)PacketEnum.PaketHeaderSize);
                    lastMsgBeginPos = (int)PacketEnum.PaketHeaderSize;
                    lastMsgEndPos = lastMsgBeginPos;
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
            lastMsgBeginPos = GetPos();
            lastMsgEndPos = lastMsgBeginPos;
            WriteNull((int)PacketEnum.MsgHeaderSize);
        }
        // 写入一个消息体结束(这里整理并完成消息头)
        public void WriteMsgOver()
        {
            int msgLen = GetPos() - lastMsgBeginPos;
            int oldPos = GetPos();
            Seek(0 - msgLen);
            WriteUint32(((uint)currMsgID << 16) | (uint)msgLen);
            Seek(msgLen);
            lastMsgEndPos = oldPos;
            ++msgCount;
        }
        // 写入一个完整的消息数据(包括消息头)
        public void WriteIntactMsgOver()
        {
            lastMsgBeginPos = GetPos();
            lastMsgEndPos = GetPos();
            ++msgCount;
        }
        public void PacketWriteOver()
        {
            if (hasPacketHeader)
            {
                SeekBegin();
                try
                {
                    WriteUint16((ushort)(GetLen()/* - (int)PacketEnum.PaketHeaderSize*/));
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