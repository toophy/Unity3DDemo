using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NetMsg
{
    class PacketReader : MsgStream
    {
        private int CurrMsgPos;
        private ushort CurrMsgLen;
        private ushort CurrMsgId;
        private ushort Count;
        private ushort LinkTgid;

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
        private const ushort packetHeaderLen = 4;
        private const ushort msgHeaderLen = 4;

        private ushort currMsgID;// 当前消息ID
        private ushort msgCount;// 包内消息总数(包括尾随消息)

        public PacketWriter(byte[] d) : base(d)
        {
            Seek
            // packetHeader
            //  : ushort packetLen
            //  : byte token
            //  : byte msgCount
            //
            // msgHeader
            //  : ushort msgLen
            //  : ushort msgId
            //
            // msgBody
        }
        public void Reset()
        {
            Clear();
            currMsgID = 0;
            msgCount = 0;
        }
        public void SetsubTgid(long id)
        {
        }
        public void WriteMsgId(ushort id)
        {
            currMsgID = id;
        }
        public void WriteMsgOver()
        {
            ++msgCount;
        }
    }
}