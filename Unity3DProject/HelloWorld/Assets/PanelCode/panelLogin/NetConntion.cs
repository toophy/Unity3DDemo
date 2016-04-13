using UnityEngine;
using System.Collections;
using NetMsg;
using System;
using System.Net;
using System.Net.Sockets;


public class NetConntion : MonoBehaviour
{
    public Socket mySocket;
    private byte[] myWriteBuffer;
    private PacketWriter myWriter;
    private byte[] myReadPacketHeader;
    private byte[] myReadPacketBody;
    


    public NetConntion()
    {
        myWriteBuffer = new byte[(int)PacketEnum.MaxWriteLen];
        myWriter = new PacketWriter(myWriteBuffer,true);
        myReadPacketHeader = new byte[(int)PacketEnum.PaketHeaderSize*2];
        myReadPacketBody = new byte[(int)PacketEnum.MaxReadLen];
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 读取数据
        //byte[] header = new byte[20];
        //myClientA.Receive(header, 2, SocketFlags.None);
        //int bodyLen = header[0] | (header[1] << 8);
        //byte[] body = new byte[4096];
        //myClientA.Receive(body, bodyLen, SocketFlags.None);

        //MsgStream stream = new MsgStream(body);
        //stream.SetCurLen(bodyLen);
        //CMsgLogin msgLoginRead = new CMsgLogin();
        //msgLoginRead.read(ref stream);

        // 定时发送
        if (myWriter.GetMsgCount() >= 0)
        {
            if (IsConnected())
            {
                myWriter.PacketWriteOver();
                byte[] tempx = new byte[(int)PacketEnum.MaxWriteLen];
                Buffer.BlockCopy(myWriteBuffer, 0, tempx, 0, myWriter.GetLen());
                StartCoroutine(OnSend(tempx, myWriter.GetLen()));
            }
        }
    }

    public void Receive()
    {
    }

    private IEnumerator OnReadPacket()
    {
        yield return mySocket.Receive(myReadPacketHeader, (int)PacketEnum.MsgHeaderSize, SocketFlags.None);
        PacketReader streamHeader = new PacketReader(myReadPacketHeader, (int)PacketEnum.MsgHeaderSize,0);

        ushort nLen = streamHeader.ReadUint16();
        ushort nToken = streamHeader.ReadUint8();
        ushort nCount = streamHeader.ReadUint8();

        yield return mySocket.Receive(myReadPacketBody, nLen, SocketFlags.None);

        PacketReader streamBody = new PacketReader(myReadPacketBody, nLen, nCount);

        for (int i = 0; i < nCount; i++)
        {
            ushort nMsgLen = streamBody.ReadUint16();
            ushort nMsgId = streamBody.ReadUint16();

            switch (nMsgId)
            {
                case (ushort)MsgId_chat_proto.G2C_login_ret_Id:
                    G2C_login_ret md = new G2C_login_ret();
                    try
                    {
                        md.Read(ref streamBody);
                        Console.WriteLine("[I] Recv G2C_login_ret({0},{1})", md.Ret, md.Msg);
                    }
                    catch (ReadWriteException e)
                    {
                        Console.WriteLine("[E] G2C_login_ret read : " + e.Message);
                    }
                    break;
                default:
                    break;
            }

        }
    }
    

    public void Connect(string ip, int port)
    {
        if (!IsConnected())
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.Connect(endPoint);
        }
    }

    public bool IsConnected()
    {
        if (mySocket != null)
        {
            return mySocket.Connected;
        }
        return false;
    }

    public void PostAmsg(byte[] buf, int size)
    {
        if (size > 0 && buf != null && size <= (int)PacketEnum.MaxWriteLen)
        {
            byte[] temp = new byte[(int)PacketEnum.MaxWriteLen];
            Buffer.BlockCopy(buf, 0, temp, 0, size);
            if (myWriter.CanWriteData(size))
            {
                //myWriter.WriteMsgId(0);
                myWriter.WriteData(temp, size);
                myWriter.WriteMsgOver();
            }
            else
            {
                // 老数据发送, 新数据库
                if (IsConnected())
                {
                    myWriter.PacketWriteOver();
                    byte[] tempx = new byte[(int)PacketEnum.MaxWriteLen];
                    Buffer.BlockCopy(myWriteBuffer, 0, tempx, 0, myWriter.GetLen());
                    StartCoroutine(OnSend(tempx, myWriter.GetLen()));
                }
                myWriter.Reset();
                myWriter.WriteData(temp, size);
                myWriter.WriteMsgOver();
            }
        }
    }

    private IEnumerator OnSend(byte[] buf, int size)
    {
        yield return mySocket.Send(buf, size, SocketFlags.None);
    }
}
