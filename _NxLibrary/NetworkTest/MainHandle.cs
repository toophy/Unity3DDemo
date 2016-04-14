using NxBase;
using NxBase.Dispatcher;
using NxNetwork;
using proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTest
{
    class MainHandle
    {
        public MainHandle()
        {
            NetworkManager.Instance.Register(new ProtoCallBack<testmsg>(onTestMsg));
        }

        public void onTestMsg(testmsg info)
        {
            NxLog.v(this, "接受到test信息：" + info.name);
        }
    }
}
