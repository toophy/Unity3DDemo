using UnityEngine;
using System.Collections;
using NetMsg;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Text;

public class NetConntion : MonoBehaviour
{
    public Socket mySocket;
    private byte[] myWriteBuffer;
    private PacketWriter myWriter;
    private byte[] myReadPacketBody;



    public NetConntion()
    {
        myWriteBuffer = new byte[(int)PacketEnum.MaxWriteLen];
        myWriter = new PacketWriter(myWriteBuffer, true);
        myReadPacketBody = new byte[(int)PacketEnum.MaxReadLen];
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 定时发送
        if (myWriter.GetMsgCount() > 0)
        {
            if (IsConnected())
            {
                myWriter.PacketWriteOver();
                byte[] tempx = new byte[(int)PacketEnum.MaxWriteLen];
                Buffer.BlockCopy(myWriteBuffer, 0, tempx, 0, myWriter.GetLen());
                //StartCoroutine(OnSend(tempx, tempLen));
                mySocket.Send(tempx, myWriter.GetLen(), SocketFlags.None);
                myWriter.Reset();
            }
        }
    }

    public void Receive()
    {
    }

    private IEnumerator OnClose(string err)
    {
        Debug.Log(err);
        yield return null;
    }

    private IEnumerator OnReadPacket()
    {
        bool bReadHeader = true;
        ushort nLen = 0;
        ushort nCount = 0;
        int nNeedReadLen = (int)PacketEnum.MsgHeaderSize;
        for (;;)
        {
            if (mySocket != null && mySocket.Connected)
            {
                if (mySocket.Available > 0)
                {
                    int nRealLen = mySocket.Receive(myReadPacketBody, nNeedReadLen, SocketFlags.None);
                    if (nRealLen != 0)
                    {
                        PacketReader stream = new PacketReader(myReadPacketBody, nNeedReadLen, 0);

                        if (bReadHeader)
                        {
                            nLen = stream.ReadUint16();
                            nCount = stream.ReadUint16();

                            bReadHeader = false;
                            nNeedReadLen = nLen - nNeedReadLen;
                            if (nNeedReadLen < (int)PacketEnum.MsgHeaderSize || nNeedReadLen > ((int)PacketEnum.MaxReadLen - (int)PacketEnum.PaketHeaderSize))
                            {
                                yield return null;
                                StartCoroutine(OnClose(string.Format("[W] 读取包头失败(Len={0},Count={1}", nLen, nCount)));
                                break;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < nCount; i++)
                            {
                                ushort nMsgLen = stream.ReadUint16();
                                ushort nMsgId = stream.ReadUint16();

                                switch (nMsgId)
                                {
                                    case (ushort)MsgId_chat_proto.G2C_login_ret_Id:
                                        G2C_login_ret md = new G2C_login_ret();
                                        try
                                        {
                                            md.Read(ref stream);

                                            //hide panel login
                                            RectTransform panelLogin;
                                            panelLogin = GameObject.Find("Canvas/Panel").GetComponent<RectTransform>();
                                            panelLogin.anchoredPosition = new Vector2(1000.0f, 1000.0f);

                                            //hide panel login
                                            RectTransform panelChat;
                                            panelChat = GameObject.Find("Canvas/PanelChat").GetComponent<RectTransform>();
                                            panelChat.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                                        }
                                        catch (ReadWriteException e)
                                        {
                                            Debug.LogFormat("[E] G2C_login_ret read : " + e.Message);
                                        }
                                        break;
                                    case (ushort)MsgId_chat_proto.S2C_chat_Id:
                                        S2C_chat md_chat = new S2C_chat();
                                        try
                                        {
                                            md_chat.Read(ref stream);

                                            //show chat
                                            Text chatText;
                                            chatText = GameObject.Find("Canvas/PanelChat/TextChat").GetComponent<Text>();
                                            chatText.text += md_chat.Data;
                                        }
                                        catch (ReadWriteException e)
                                        {
                                            Debug.LogFormat("[E] S2C_chat read : " + e.Message);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            bReadHeader = true;
                            nNeedReadLen = (int)PacketEnum.MsgHeaderSize;
                        }
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }


    public void Connect(string ip, int port)
    {
        if (!IsConnected())
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mySocket.NoDelay = true;
            mySocket.Connect(endPoint);
            mySocket.Blocking = false;

            StartCoroutine(OnReadPacket());
            Debug.LogFormat("[D] Connect {0}:{1} ok", ip, port);
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
                myWriter.WriteIntactMsgOver();
            }
            else
            {
                // 老数据发送, 新数据库
                if (IsConnected())
                {
                    myWriter.PacketWriteOver();
                    byte[] tempx = new byte[(int)PacketEnum.MaxWriteLen];
                    int tempLen = myWriter.GetLen();
                    Buffer.BlockCopy(myWriteBuffer, 0, tempx, 0, myWriter.GetLen());
                    //StartCoroutine(OnSend(tempx, myWriter.GetLen()));
                    mySocket.Send(tempx, myWriter.GetLen(), SocketFlags.None);
                }
                myWriter.Reset();
                myWriter.WriteData(temp, size);
                myWriter.WriteIntactMsgOver();
            }
        }
    }

    private IEnumerator OnSend(byte[] buf, int size)
    {
        mySocket.Send(buf, size, SocketFlags.None);
        yield return new WaitForEndOfFrame();
    }
}

