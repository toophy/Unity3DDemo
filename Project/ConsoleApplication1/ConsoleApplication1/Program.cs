using System;
using NetMsg;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        // 线程，负责一个网络端口的读写

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10234);
            Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.Bind(endPoint);
            mySocket.Listen(3);
            Socket myClientA = mySocket.Accept();
            byte[] header = new byte[20];
            myClientA.Receive(header, 2, SocketFlags.None);
            int bodyLen = header[0] | (header[1] << 8);
            byte[] body = new byte[4096];
            myClientA.Receive(body, bodyLen, SocketFlags.None);

            MsgStream stream = new MsgStream(body);
            stream.SetCurLen(bodyLen);
            CMsgLogin msgLoginRead = new CMsgLogin();
            msgLoginRead.read(ref stream);

            Console.WriteLine(msgLoginRead.key);
            Console.WriteLine(msgLoginRead.account);
            Console.WriteLine(msgLoginRead.sid);
            Console.WriteLine(msgLoginRead.sign);
            Console.WriteLine(msgLoginRead.height);
            Console.WriteLine(msgLoginRead.age);
            Console.WriteLine(msgLoginRead.job);
            Console.WriteLine(msgLoginRead.hp);
            Console.WriteLine(msgLoginRead.mp);


            //Console.ReadLine();

            //byte[] buffer = new byte[4096];
            //MsgStream stream = new MsgStream(buffer);
            //// write
            //CMsgLogin msgLogin = new CMsgLogin();
            //msgLogin.key = 99;
            //msgLogin.account = "6998_5_liusl";
            //msgLogin.sid = "5";
            //msgLogin.sign = "sldkjfslkjfd";
            //msgLogin.height = 168.33223f;
            //msgLogin.age = 10016.230927231;
            //msgLogin.job = 233232;
            //msgLogin.hp = 2342352234;
            //msgLogin.mp = 283792394;

            //msgLogin.write(ref stream);

            //// read
            //stream.SeekBegin();

            //CMsgLogin msgLoginRead = new CMsgLogin();
            //msgLoginRead.read(ref stream);

            //Console.WriteLine(msgLoginRead.key);
            //Console.WriteLine(msgLoginRead.account);
            //Console.WriteLine(msgLoginRead.sid);
            //Console.WriteLine(msgLoginRead.sign);
            //Console.WriteLine(msgLoginRead.height);
            //Console.WriteLine(msgLoginRead.age);
            //Console.WriteLine(msgLoginRead.job);
            //Console.WriteLine(msgLoginRead.hp);
            //Console.WriteLine(msgLoginRead.mp);


            Console.ReadLine();
        }
    }
}
