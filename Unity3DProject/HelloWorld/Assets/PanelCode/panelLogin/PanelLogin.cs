using UnityEngine;
using System.Xml.Linq;
using UnityEngine.UI;
using System.Collections;
using NetMsg;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

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

    public void OnClickBtnOk()
    {
        if (!myConn.IsConnected())
        {
            myConn.Connect("127.0.0.1", 9998);
            if (myConn.IsConnected())
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
                myConn.PostAmsg(wstream.GetData(), wstream.GetLen());

                Scene uiScene = SceneManager.GetActiveScene();
                //Scene city1Scene = SceneManager.CreateScene("city_1");
                //SceneManager.LoadScene("city_1",LoadSceneMode.Additive);
            }
        }
    }
}
