using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System;
using UnityEngine.UI;



public class socketScript : MonoBehaviour {
	
	//variables
	
	public static socketScript self;
	
	private bool connected;
	public bool setup;
	
	private TCPConnection myTCP;
	
	private string msgToSend;
	private string msgReceived;

	
	void Awake() {
		
		//add a copy of TCPConnection to this game object
		if (self == null)
			self = this;
		else
			Destroy(this);
		
		myTCP = gameObject.AddComponent<TCPConnection>();
		
		
	}
	
	
	
	void Start () {

	
	}
	
	void Update () {
	
		if (myTCP.socketReady == false) 
		{
						
			myTCP.setupSocket();
			
		}
		else
		{
		//keep checking the server for messages, if a message is received from server, it gets logged in the Debug console (see function below)
		SocketResponse ();
		}
			
	}
	public void SendMessage()
	{
		SendToServer("Teste "+ System.DateTime.Now);
	
	}
	
	
	//socket reading script
	
	void SocketResponse() {
		
		string serverSays = myTCP.readSocket();
			
		if (serverSays != "") {

			handleRequest(serverSays);
	
		}
		
		
		
	}
    public string commaToPoint(string received)
    {

        string response ="";
        char[] tempchar = received.ToCharArray();

        for(int i=0; i<tempchar.Length;i++)
        {
            if (tempchar[i] == ',')
                response = response + ".";
            else
                response = response + tempchar[i].ToString();

        }
        return response;
    }
	public void handleRequest(string p_s) //separar leitura arquivo obj
	{
		
		char[] delimiters = { '(','/',')',' ',',' };
		List<string> tempStr = new List<string>();
		Debug.Log("received" + p_s);
		string[] words = p_s.Split(delimiters);
		
		for (int i=0; i< words.Length; i++)
		{
			Debug.Log(words[i]);			

		}
		
	}
	
	public void SendToServer(string str) 
	{
		
		myTCP.writeSocket(str);
		Debug.Log("sent" + str);

	}
	
	
	
}
