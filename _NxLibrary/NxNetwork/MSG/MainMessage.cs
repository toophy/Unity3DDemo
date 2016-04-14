using NxBase;
using NxBase.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NxNetwork.MSG
{
    [Serializable, StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public struct MainMessage
    {
        public MainMessage(int lenght, long timestamp, string msgName, byte[] msgbuffer)
        {
            _lenght = lenght;
            _timestamp = timestamp;
            _msgbuffer = msgbuffer;
            _msgName = msgName;
        }

        int _lenght;
        public int Lenght
        {
            get { return _lenght; }
        }

        long _timestamp;
        public long Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                _timestamp = value;
            }
        }

        string _msgName;
        public string MsgName
        {
            get { return _msgName; }
            set { _msgName = value; }
        }

        byte[] _msgbuffer;
        public byte[] MsgBuffer
        {
            get { return _msgbuffer; }
        }

        public T GetMsgContent<T>()
        {
            return SerializationHelper.ProtoDeSerialization<T>(MsgBuffer);
        }

        public byte[] ToBinary()
        {
            return SerializationHelper.BinarySerialization(this);
        }
    }
}
