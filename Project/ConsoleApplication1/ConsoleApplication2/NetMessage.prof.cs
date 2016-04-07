using NetMsg;
using System;

namespace NetMsg
{
    class CMsgLogin
    {
        public int key;
        public string account;
        public string sid;
        public string sign;
        public float height;
        public double age;
        public int job;
        public long hp;
        public long mp;

        public void read(ref MsgStream stream)
        {
            try
            {
                key = stream.ReadInt32();
                stream.ReadString(ref account);
                stream.ReadString(ref sid);
                stream.ReadString(ref sign);
                height = stream.ReadFloat32();
                age = stream.ReadFloat64();
                job = stream.ReadInt24();
                hp = stream.ReadInt56();
                mp = stream.ReadInt48();
            }
            catch (ReadWriteException e)
            {
                Console.WriteLine("[E] CMsgLogin read : " + e.Message);
            }
        }

        public void write(ref MsgStream stream)
        {
            try
            {
                stream.WriteInt32(key);
                stream.WriteString(ref account);
                stream.WriteString(ref sid);
                stream.WriteString(ref sign);
                stream.WriteFloat32(height);
                stream.WriteFloat64(age);
                stream.WriteInt24(job);
                stream.WriteInt56(hp);
                stream.WriteInt48(mp);
            }
            catch (ReadWriteException e)
            {
                Console.WriteLine("[E] CMsgLogin write : " + e.Message);
            }
        }
    }
}
