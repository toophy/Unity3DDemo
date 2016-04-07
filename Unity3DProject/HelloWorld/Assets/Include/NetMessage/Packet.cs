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

        public PacketReader(byte[] d, ushort count):base(d)
        {
            CurrMsgId = 0;
            CurrMsgLen = 0;
            CurrMsgPos = 0;
            Count = count;
        }

        public void PreReadMsg(ushort msg_id,ushort msg_len,int start_pos)
        {
            CurrMsgId = msg_id;
            CurrMsgLen = msg_len;
            CurrMsgPos = start_pos;
        }
    }

    class PacketWriter : MsgStream
    {
        public PacketWriter(byte[] d):base(d)
        {
        }
        public void SetsubTgid(long id)
        {
        }
        public void WriteMsgId(ushort id)
        {
        }
        public void WriteMsgOver()
        {
        }
    }
}