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

            PacketWriter wstream = new PacketWriter(false);
            C2G_login msgLogin = new C2G_login();
            msgLogin.Account = inputText.text;
            msgLogin.Time = 0;
            msgLogin.Sign = "pwd";
            msgLogin.Write(ref wstream);
            myConn.PostAmsg(wstream.GetData(),wstream.GetLen());

            Debug.Log(msgLogin.Account);
        }
    }
}
