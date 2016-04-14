using NxBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase.Dispatcher
{
    public class NxDispatcher
    {
        private Dictionary<string, ICallback<Object>> _callBackDic = new Dictionary<string, ICallback<Object>>();
        private ICallback<Object> _curCallBack = null;

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Register<T>(ICallback<T> callback)
        {
            _callBackDic[DispatcherHelper.GetKey<T>()] = (ICallback<Object>)callback;
            return false;
        }

        /// <summary>
        /// 分发消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Dispatcher(string key, byte[] data)
        {
            if (_callBackDic.TryGetValue(key, out _curCallBack))
            {
                _curCallBack.Exce(data);
            }
        }
    }
}
