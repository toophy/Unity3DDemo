using UnityEngine;
using System.Collections;
using NetMsg;
using System;
using System.Net;
using System.Net.Sockets;


public class NetConntion : MonoBehaviour
{
    public Socket mySocket;
    private byte[] myBuffer;
    private PacketWriter myWriter;


    public NetConntion()
    {
        myBuffer = new byte[(int)PacketEnum.MaxWriteLen];
        myWriter = new PacketWriter(myBuffer,true);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 定时发送
        if (myWriter.GetMsgCount() >= 0)
        {
            if (IsConnected())
            {
                myWriter.PacketWriteOver();
                byte[] tempx = new byte[(int)PacketEnum.MaxWriteLen];
                Buffer.BlockCopy(myBuffer, 0, tempx, 0, myWriter.GetLen());
                StartCoroutine(OnSend(tempx, myWriter.GetLen()));
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
                    Buffer.BlockCopy(myBuffer, 0, tempx, 0, myWriter.GetLen());
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
