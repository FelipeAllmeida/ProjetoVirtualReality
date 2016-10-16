using UnityEngine;
using System.Collections;

public class DataPacketServer : MonoBehaviour 
{

	public int serial;
	public Vector3 position;
	public Vector3 rotation;
	public int type;


	public string returnData()
	{
		position = transform.position;
		rotation = transform.eulerAngles;

		string resultString = "";
		resultString = resultString      + serial
									+"|" + type
									+"|" + position
									+"|" + rotation;

		return resultString;
	}


}
