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
        myBuffer = new byte[4096];
        myWriter = new PacketWriter(myBuffer);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 定时发送
        if (myWriter.GetPos()>=6)
        {
            byte[] sendData = new byte[4096];
            Buffer.BlockCopy(myBuffer, 0, sendData, 0, myWriter.GetPos());
            Send(sendData, myWriter.GetPos());
            myWriter.Reset();
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

    private void Send(byte[] buf, int size)
    {
        if (IsConnected())
            StartCoroutine(OnSend(buf, size));
    }

    private IEnumerator OnSend(byte[] buf, int size)
    {
        yield return mySocket.Send(buf, size, SocketFlags.None);
    }
}
