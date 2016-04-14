using NxBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase.Dispatcher
{
    public class ProtoCallBack<T> : ICallback<T>
    {
        private Action<T> _handle = null;

        public ProtoCallBack(Action<T> handle)
        {
            _handle = handle;
        }

        public void Exce(byte[] data)
        {
            var tData = GetData(data);
            exce(tData);
        }

        public T GetData(byte[] data)
        {
            return SerializationHelper.ProtoDeSerialization<T>(data);
        }

        public void exce(T data)
        {
            if (_handle != null)
            {
                _handle(data);
            }
        }
    }
}
