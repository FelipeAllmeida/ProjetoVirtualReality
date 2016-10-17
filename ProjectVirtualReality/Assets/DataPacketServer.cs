using UnityEngine;
using System.Collections;
using System;

public class DataPacketServer : MonoBehaviour 
{

	public int serial;
	public Vector3 position;
	public Vector3 rotation;
	public int type;
	public Action<DataPacketServer> onDestroy;


	public string returnData()
	{
		position = transform.position;
		rotation = transform.eulerAngles;

		string resultString = "";
		resultString = resultString      + serial
									+"|" + type
									+"|" + Math.Round(position.x,6) + "," + Math.Round(position.y,6) + "," + Math.Round(position.z,6)
									+"|" + Math.Round(rotation.x,6) + "," + Math.Round(rotation.y,6) + "," + Math.Round(rotation.z,6);

		return resultString;
	}
	public void HandlerDestroy()
	{
		onDestroy(this);

	}

}
