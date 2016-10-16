using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class TCPConnection : MonoBehaviour {
	
	//the name of the connection, not required but better for overview if you have more than 1 connections running
	public string conName = "Localhost";
	
	
	//port for the server, make sure to unblock this in your router firewall if you want to allow external connections
	public int conPort = 123;
	
	//a true/false variable for connection status
	public bool socketReady = false;
	
	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;

	private string _host;
	private int _port;
	
	//try to initiate connection
	public void setupSocket(string host, int port) {
	
		_host = host;
		_port = port;

		try {
			mySocket = new TcpClient(_host, _port);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}
	
	//send message to server
	public void writeSocket(string theLine) {
		if (!socketReady)
			return;
		String tmpString = theLine + "\r\n";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}
	
	//read message from server
	public string readSocket() {
		String result = "";
		try
		{
		if (theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			result += System.Text.Encoding.UTF8.GetString(inStream);
		}
		}
		catch
		{
		
		}
		return result;
	}
	
	//disconnect from the socket
	public void closeSocket() {
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}
	
	//keep connection alive, reconnect if connection lost
	public void maintainConnection(){
		if(!theStream.CanRead) {
			setupSocket(_host, _port);
		}
	}
	
	
}