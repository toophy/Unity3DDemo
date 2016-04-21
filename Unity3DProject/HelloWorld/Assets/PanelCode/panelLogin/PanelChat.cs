using UnityEngine;
using UnityEngine.UI;
using NetMsg;
using System.Collections;

public class PanelChat : MonoBehaviour
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
    public void OnClickBtnSend()
    {
        //inputUserName
        Text inputText;
        inputText = GameObject.Find("Canvas/PanelChat/InputChat/Text").GetComponent<Text>();

        PacketWriter wstream = new PacketWriter(false);
        C2S_chat msgChat = new C2S_chat();
        msgChat.Channel = 1;
        msgChat.Data = inputText.text;
        msgChat.Write(ref wstream);
        myConn.PostAmsg(wstream.GetData(), wstream.GetLen());

        
    }
}
