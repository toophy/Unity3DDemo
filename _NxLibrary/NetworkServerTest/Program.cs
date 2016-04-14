using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using NxNetwork;
using NxBase;
using login;
using NxNetwork.MSG;
using NxBase.Helper;

namespace NetworkServerTest
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private static int myProt = 8885;   //端口
        static Socket serverSocket;
        static void Main(string[] args)
        {
                        //服务器IP地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口
            serverSocket.Listen(10);    //设定最多10个排队连接请求
            NxLog.v(serverSocket, "启动监听" + serverSocket.LocalEndPoint.ToString() + "成功");
            //通过Clientsoket发送数据
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();
        }

        
        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();

                UserInfo info = new UserInfo();
                info.accid = "1234";
                info.pwd = "aaaa";
                var data = SerializationHelper.ProtoSerialization(info);
                MainMessage msg = new MainMessage(data.Length, DateTime.Now.ToBinary(), info.GetType().Name, data);
                var msgData = msg.ToBinary();
                NxLog.v<Program>("发送数据给客户端，数据长度:" + msgData.Length);
                clientSocket.Send(msgData);
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="clientSocket"></param>
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据
                    int receiveNumber = myClientSocket.Receive(result);
                    NxLog.v(myClientSocket, "接收客户端" + myClientSocket.RemoteEndPoint.ToString() + "消息");

                    //给客户端发回去
                    myClientSocket.Send(result);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}
