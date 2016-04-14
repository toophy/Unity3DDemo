using NxNetwork;
using NxBase;
using login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto;
using NxBase.Dispatcher;

namespace NetworkTest
{
    class LoginHandle
    {
        public LoginHandle()
        {
            NetworkManager.Instance.Register(new ProtoCallBack<UserInfo>(onUserInfo));
        }

        public void onUserInfo(UserInfo info)
        {
            NxLog.v(this, "接受到用户信息：" + info.accid + "密码：" + info.pwd);
        }
    } 
}
