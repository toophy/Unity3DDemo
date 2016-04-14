using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NxBase.Helper
{
    /// <summary>
    /// 包装一下各种序列化
    /// </summary>
    public class SerializationHelper
    {
        /// <summary>
        /// protobuf的序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ProtoSerialization<T>(T data)
        {
            byte[] bData = null;
            using(MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, data);
                bData = new byte[ms.Length];
                Array.Copy(ms.GetBuffer(), bData, ms.Length);
            }
            return bData;
        }

        /// <summary>
        /// protobuf的反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ProtoDeSerialization<T>(byte[] data)
        {
            T t;
            using (MemoryStream ms = new MemoryStream(data))
            {
                t = Serializer.Deserialize<T>(ms);
            }
            return t;
        }

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] BinarySerialization<T>(T data)
        {
            byte[] bData = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);
                bData = stream.GetBuffer();
            }
            return bData;
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T BinaryDeSerialization<T>(byte[] data)
        {
            T t;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                t = (T)formatter.Deserialize(stream);
            }
            return t;
        }
    }
}
