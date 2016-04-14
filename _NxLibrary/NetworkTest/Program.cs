using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NxNetwork;
using NxBase;
using System.Threading;
using System.Collections;
using proto;

namespace NetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkManager.Instance.Init();
            NetworkManager.Instance.ConnectServer("127.0.0.1", 8885);

            NxTask.Instance.AddTask(SendMessage());

            NxLog.v<Program>("init");
            LoginHandle handle = new LoginHandle();
            MainHandle handle2 = new MainHandle();

            while(true)
            {
                NxTask.Instance.DoTask();
            }
        }

        public void onLogin(login.UserInfo data)
        {
            NxLog.v<Program>("aaaaaa");
        }

        private static IEnumerator SendMessage()
        {
            List<string> nameList = new List<string>();
            nameList.Add("消息1");
            nameList.Add("消息2");
            nameList.Add("消息3");
            var rand = new Random((int)DateTime.Now.ToBinary());
            while (true)
            {
                int i = rand.Next(3);
                yield return new WaitForSecond(new TimeSpan(0, 0, 5));

                testmsg msg = new testmsg();
                msg.name = nameList.ElementAt(i);
                hand h = new hand();
                h.dir = 1;
                msg.thehand.Add(h);
                NetworkManager.Instance.SendMsg(msg);
            }
        }
    }
}
