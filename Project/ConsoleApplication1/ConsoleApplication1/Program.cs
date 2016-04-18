using System;
using NetMsg;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace ConsoleApplication1
{
    public class NetConnction
    {
        private int myId;
        private Socket mySocket;
        private byte[] myReadBuff;


        public NetConnction(Socket s)
        {
            mySocket = s;
            read_write_state = 0;
            myReadBuff = new byte[4096];
        }

        public override string ToString()
        {
            return "NetConnction";
        }

        public bool Connect(string ip, int port)
        {
            return true;
        }

        public IEnumerator Read()
        {
            yield return OnRead();
        }

        public NetConnction OnRead()
        {
            mySocket.Receive(myReadBuff);
            read_write_state = 1;
            return this;
        }

        public IEnumerator Write(byte[] buff, int buff_size)
        {
            yield return OnWrite(buff, buff_size);
        }

        public NetConnction OnWrite(byte[] buff, int buff_size)
        {
            mySocket.Send(buff, buff_size, SocketFlags.None);
            read_write_state = 2;
            return this;
        }

        public bool isReadState()
        {
            return read_write_state == 1;
        }

        public bool isWriteState()
        {
            return read_write_state == 2;
        }

        private int read_write_state;


    }

    public class NetListenor : IEnumerable
    {
        private Socket mySocket;
        private IPEndPoint endPoint;
        private List<NetConnction> conns;

        public NetListenor()
        {
            conns = new List<NetConnction>();
        }

        public IEnumerator GetEnumerator()
        {
            yield return null;
        }

        public bool Listen(string ip, int port)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.Bind(endPoint);
            mySocket.Listen(3);
            return true;
        }

        public void WaitAccepts()
        {
        }

        public IEnumerator StartAccept()
        {
            yield return OnAccept();
        }

        public string OnAccept()
        {
            Socket myClientA = mySocket.Accept();
            NetConnction conn = new NetConnction(myClientA);
            conns.Add(conn);
            Console.WriteLine("[I] 接收到一个网络连接");
            return "ac";
        }

        public void OnUpdate()
        {
            IEnumerator enumerator = StartAccept();
            while (enumerator.MoveNext())
            {
                switch (enumerator.ToString())
                {
                    case "NetConnction":
                        NetConnction netConn = enumerator as NetConnction;
                        if (netConn.isReadState())
                        {
                            netConn.Read();
                        }
                        break;
                    case "string":
                        enumerator = StartAccept();
                        break;
                }
            }
        }

    }

    public class GameMoves
    {
        // 两个迭代器，每个都有自己的While,Current, MoveNext调用迭代器自己，yield语句还会导致记录迭代器state状态。  
        private IEnumerator cross;
        private IEnumerator circle;

        public GameMoves()
        {
            // 迭代器就是一个返回枚举集合的方法，这里已经声明了，这里两个是固定的只是外部的会切换  
            cross = Cross2();
            circle = Circle();
        }

        private int move = 0;
        const int MaxMoves = 9;

        // 迭代器MoveNext会进来这里，也就是进来迭代器本身遍历获取对象  
        public IEnumerator Cross2()
        {
            while (true)
            {
                Console.WriteLine("Cross, move {0}", move);
                if (++move >= MaxMoves)
                    yield break;
                yield return circle;
            }
        }

        public IEnumerator Circle()
        {
            while (true)
            {
                Console.WriteLine("Circle, move {0}", move);
                if (++move >= MaxMoves)
                    yield break;
                yield return cross;
            }
        }
    }

    public interface ITestBase
    {
        void Print();
        IEnumerator<string> GetEnumerator();
        IEnumerable<string> Hehe();
    }

    class TestA : ITestBase
    {
        public void Print()
        {
            Console.WriteLine("TestA");
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return "TestA .1";
            yield return "TestA .2";
            yield return "TestA .3";
        }

        public IEnumerable<string> Hehe()
        {
            yield return "TestA hehe.1";
            yield return "TestA hehe.2";
            yield return "TestA hehe.3";
        }
    }

    [Serializable]
    class TestB : ITestBase
    {
        public int Age { private get; set; }
        [NonSerialized]
        private IEnumerator cross;
        [NonSerialized]
        private IEnumerator circle;
        private int move = 0;
        private const int maxMove = 9;


        public TestB()
        {
            cross = Cross();
            circle = Circle();
        }

        public void Print()
        {
            Console.WriteLine("TestB");
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return "TestB .1";
            yield return "TestB .2";
            yield return "TestB .3";
        }

        public IEnumerable<string> Hehe()
        {
            yield return "TestB hehe.1";
            yield return "TestB hehe.2";
            yield return "TestB hehe.3";
        }

        public IEnumerator Cross()
        {
            while (true)
            {
                ++move;
                Console.WriteLine("Cross {0}", move);
                if (move >= maxMove)
                {
                    yield break;
                }
                yield return circle;
            }
        }

        public IEnumerator Circle()
        {
            while (true)
            {
                ++move;
                Console.WriteLine("Circle {0}", move);
                if (move >= maxMove)
                {
                    yield break;
                }
                yield return cross;
            }
        }

    }

    class CarEvent : EventArgs
    {
        public CarEvent(string n)
        {
            Name = n;
        }

        public string Name { get; private set; }
    }

    class CarFactory
    {
        public CarFactory()
        {
        }

        public event EventHandler<CarEvent> NewCarEvent;

        public void NewCar(string n)
        {
            if (NewCarEvent != null)
            {
                NewCarEvent(this, new CarEvent(n));
            }
        }
    }

    class Customer
    {
        public string Name { get; private set; }

        public Customer(string n)
        {
            Name = n;
        }

        public void NewCarInHere(object o, CarEvent e)
        {
            Console.WriteLine("Car {0} in here {1} ", e.Name, Name);
        }
    }

    class Program
    {
        // 线程，负责一个网络端口的读写
        static int Add(int a, int b)
        {
            return a + b;
        }

        static void Main(string[] args)
        {
            var myList = new List<int>();
            myList.Add(1);
            myList.Add(2);

            foreach (var item in myList)
            {
                Console.WriteLine(item);
            }


            TestB x = new TestB();
            ITestBase[] xi = new ITestBase[2];
            xi[0] = new TestA();
            var tB = new TestB();
            xi[1] = tB;
            foreach (var item in xi)
            {
                item.Print();
                Console.WriteLine(item.ToString());
            }
            foreach (var item in xi[0])
            {
                Console.WriteLine(item);
            }
            foreach (var item in xi[1])
            {
                Console.WriteLine(item);
            }
            foreach (var item in xi[1].Hehe())
            {
                Console.WriteLine(item);
            }
            IEnumerator emt = tB.Cross();
            while (emt.MoveNext())
            {
                emt = emt.Current as IEnumerator;
            }

            IFormatter fm = new BinaryFormatter();
            Stream stream = new FileStream("myTestB.txt", FileMode.Create);
            fm.Serialize(stream, tB);
            stream.Flush();
            stream.Close();


            //

            Func<int, int, int> xo = Add;
            Console.WriteLine(xo(1, 2));

            Func<int, int, int, int> xb = (a, b, c) =>
            {
                return a - b;
            };

            Console.WriteLine(xb(1, 2, 3));

            Action<int> xa = a =>
            {
                Console.WriteLine(a);
            };

            xa(300);

            CarFactory cf = new CarFactory();

            Customer mick = new Customer("mick");
            Customer dock = new Customer("dock");

            cf.NewCarEvent += mick.NewCarInHere;
            cf.NewCarEvent += dock.NewCarInHere;

            cf.NewCar("niku");

            cf.NewCarEvent -= mick.NewCarInHere;
            //cf.NewCarEvent -= dock.NewCarInHere;

            cf.NewCar("oodxx");

            Console.ReadLine();
            //// 只是声明不会调用  
            //var game = new GameMoves();
            //// 外部的迭代器类型声明为Cross2，声明时候只是声明不会调用  
            //IEnumerator enumerator = game.Cross2();
            //// 会进入枚举器  
            //// 只是调用当前迭代器，当前迭代器返回值赋值给了当前的enumerator.Current，值是另外一个迭代器  
            //while (enumerator.MoveNext())
            //{
            //    // 用另外迭代器换掉当前迭代器  
            //    enumerator = enumerator.Current as IEnumerator;
            //}

            //NetListenor listenor = new NetListenor();
            //listenor.Listen("127.0.0.1", 12345);
            //for (;;)
            //{
            //    listenor.OnUpdate();
            //    Thread.Sleep(100);
            //}


            //byte[] header = new byte[20];
            //myClientA.Receive(header, 2, SocketFlags.None);
            //int bodyLen = header[0] | (header[1] << 8);
            //byte[] body = new byte[4096];
            //myClientA.Receive(body, bodyLen, SocketFlags.None);

            //MsgStream stream = new MsgStream(body);
            //stream.SetCurLen(bodyLen);
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
        }
    }
}
