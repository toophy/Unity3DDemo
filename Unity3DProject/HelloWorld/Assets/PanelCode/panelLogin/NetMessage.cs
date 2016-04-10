using UnityEngine;
using System.Collections;

public class NetMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Say(string s)
    {
        Debug.Log("Hello " + s);
    }
}
