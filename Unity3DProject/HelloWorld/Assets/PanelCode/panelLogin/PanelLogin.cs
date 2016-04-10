using UnityEngine;
using System.Collections;
using NetMsg;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class PanelLogin : MonoBehaviour
{
    private NetConntion myConn;
    public NetMessage myNetMsg;

    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.Find("NetMessagePanel");
        myConn = (NetConntion)go.GetComponent(typeof(NetConntion));
        myNetMsg = (NetMessage)go.GetComponent(typeof(NetMessage));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Send(Socket mySocket, byte[] buf, int size)
    {
           StartCoroutine(OnSend(mySocket, buf, size));
    }

    private IEnumerator OnSend(Socket mySocket, byte[] buf, int size)
    {
        yield return mySocket.Send(buf, size, SocketFlags.None);
    }

    public void OnClickBtnOk()
    {
        myNetMsg.Say("新的行程");
        if (!myConn.IsConnected())
        {
            myConn.Connect("127.0.0.1", 9998);
        }
        else if (myConn.IsConnected())
        {
            //inputUserName
            Text inputText;
            inputText = GameObject.Find("Canvas/Panel/inputUserName/Text").GetComponent<Text>();

            byte[] buffer = new byte[4096];
            PacketWriter wstream = new PacketWriter(buffer);
            // write
            C2G_login msgLogin = new C2G_login();
            msgLogin.Account = inputText.text;
            msgLogin.Time = 0;
            msgLogin.Sign = "pwd";
            msgLogin.Write(ref wstream);

            Debug.Log(msgLogin.Account);

            byte[] header = new byte[4096];
            MsgStream streamH = new MsgStream(header);
            try
            {
                streamH.WriteInt16((short)(wstream.GetPos() + 6));
                streamH.WriteInt8(0);
                streamH.WriteInt8(1);
                streamH.WriteInt16((short)wstream.GetPos());
                streamH.WriteData(wstream.GetData(), wstream.GetPos());
                myConn.Send(streamH.GetData(), streamH.GetPos());
                //myConn.mySocket.Send(streamH.GetData(), streamH.GetPos(), SocketFlags.None);
            }
            catch (ReadWriteException e)
            {
                Debug.Log("[E] CMsgLogin read : " + e.Message);
            }


            //    //myConn.mySocket.Send(streamH.GetData(), streamH.GetPos(), SocketFlags.None);
            //    Debug.Log("Send Message");
            //    //myConn.mySocket.Send(streamH.GetData(), streamH.GetPos(), SocketFlags.None);
            //    //myConn.mySocket.Send(wstream.GetData(), wstream.GetPos(), SocketFlags.None);
            //}
        }
    }
}
