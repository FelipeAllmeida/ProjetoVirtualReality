using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System;
using UnityEngine.UI;



public class ConnectionScript : MonoBehaviour {
	
	//variables
	
	public static ConnectionScript _self;
	
	private bool _connected;
	
	private bool _isReady;
	private bool _isServer;
	
	private TCPConnection myTCP;
	
	private string msgToSend;
	private string msgReceived;
	Process myProcess;

	private string _host;
	private int _port;

	
	public void AInitialize(bool isServer, string host, int port) {
		_isServer = isServer;
		_host = host;
		_port = port;
		if(_isServer)
		{
			
			myProcess = new Process();
			myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
			myProcess.StartInfo.CreateNoWindow = true;
			myProcess.StartInfo.UseShellExecute = false;
			String[] tempString = Application.dataPath.Split('/');
			String FullPath= "";
			for (int i=0; i<tempString.Length; i++)
			{
				if (tempString[i] != "Assets")
					FullPath = FullPath +tempString[i]+ "/";
				else
					UnityEngine.Debug.Log("?");
				
			
			}
			myProcess.StartInfo.FileName = FullPath + "Resources/ServerApp/ServerApp/bin/Debug/ServerApp.exe";

			try
			{				
   				
			//	string path = "C:\\Users\\Brian\\Desktop\\testFile.bat";
			//	myProcess.StartInfo.Arguments = "/c" + path;
			//	myProcess.EnableRaisingEvents = true;
				myProcess.Start();
			//	myProcess.WaitForExit();
			//	int ExitCode = myProcess.ExitCode;
				

			}
			catch (Exception e)
			{
			
				UnityEngine.Debug.LogError(e);

			}
			if (myProcess.Responding)
				myProcess.CancelOutputRead();
		}

		//add a copy of TCPConnection to this game object
		if (_self == null)
			_self = this;
		else
			Destroy(this);
		
		myTCP = gameObject.AddComponent<TCPConnection>();
		
      }

	

	
	public void AUpdate (String p_message) {
	
		if (myTCP.socketReady == false) 
		{
			
			myTCP.setupSocket(_host,_port);
			
		}
		else
		{
		//keep checking the server for messages, if a message is received from server, it gets logged in the Debug console (see function below)
		SendMessage(p_message);
		SocketResponse ();
		}
			
	}
	public void SendMessage(String p_message)
	{
		SendToServer(p_message +" "+ System.DateTime.Now);
	
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
		UnityEngine.Debug.Log("received" + p_s);
		string[] words = p_s.Split(delimiters);
		
		for (int i=0; i< words.Length; i++)
		{
			UnityEngine.Debug.Log(words[i]);			

		}
		
	}
	
	public void SendToServer(string str) 
	{
		
		myTCP.writeSocket(str);
		UnityEngine.Debug.Log("sent" + str);

	}
	
	
	
}
