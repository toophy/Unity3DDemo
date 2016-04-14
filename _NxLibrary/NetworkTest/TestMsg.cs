using NxBase.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTest
{
    [Serializable]
    class TestMsg
    {
        public string aa;
        public string bb;
    }

    class MsgHandle
    {
        public MsgHandle(NxDispatcher dispatcher)
        {
            dispatcher.Register(new ProtoCallBack<TestMsg>(OnTestMsg));
        }

        public void OnTestMsg(TestMsg msg)
        {
            NxBase.NxLog.v(this, "aa=" + msg.aa + "bb=" + msg.bb);
        }
    }
}
