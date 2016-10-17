
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateTank : MonoBehaviour 
{
    #region Serializable Data
    [Header("Transforms")]
    [SerializeField] private Transform _static;
    [SerializeField] private Transform _dynamic;
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabPlayer;
    [Header("User Interface")]
    [SerializeField] private UITank _userInterface;
	[Header("Connection")]
	[SerializeField] private bool connect;
	[SerializeField] private string _ip;
	[SerializeField] private int _port;
	[SerializeField]private GameObject[] prefabsForeignObjects;
    #endregion

    #region Private Data
    private TankPlayer _player;
	private ConnectionScript _connectionScript;
	private List<DataPacketServer> _dataPackets;
	private List<DataPacketServer> _foreignDataPackets;
	private int serialData;
    #endregion

	public struct hudValues
	{

		public float reloadTime;
		public float speed;
		public float rotation;
		public float turretRotation;
		public float health;
		public int ammo;

	}

    private void Start () 
    {
		_dataPackets = new List<DataPacketServer>();	
_foreignDataPackets = new List<DataPacketServer>();	
		serialData = 0;
        InitializePlayer();
        InitializeUserInterface();
		if (connect)
			InitializeConnection();

    }	
	private void InitializeConnection()
	{

		_connectionScript = transform.gameObject.AddComponent<ConnectionScript>();
		_connectionScript.AInitialize(false,_ip,_port);

	}

    private void InitializePlayer()
    {
        GameObject __playerGameObject = SpawnerManager.SpawnAt(_prefabPlayer, new Vector3(0f, 0f, 0f), _dynamic, new Quaternion(0f, 0f, 0f, 0f));
        _player = __playerGameObject.GetComponent<TankPlayer>();


		DataPacketServer __dataPacketTank = (__playerGameObject.AddComponent<DataPacketServer>());
		__dataPacketTank.serial = serialData;
		__dataPacketTank.type = 0;
		serialData++;

		GameObject __turretGO = __playerGameObject.GetComponent<TankPlayer>()._turret.gameObject;
		DataPacketServer __dataPacketTurret = (__turretGO.AddComponent<DataPacketServer>());
		__dataPacketTurret.serial = serialData;
		__dataPacketTurret.type = 1;
		serialData++;

		GameObject __gunGO = __playerGameObject.GetComponent<TankPlayer>()._gun.gameObject;
		DataPacketServer __dataPacketGun = (__gunGO.AddComponent<DataPacketServer>());
		__dataPacketGun.serial = serialData;
		__dataPacketGun.type = 2;
		serialData++;

			
		_dataPackets.Add(__dataPacketTank);
		_dataPackets.Add(__dataPacketTurret);
		_dataPackets.Add(__dataPacketGun);

     	_player.AInitialize();

    }

    private void InitializeUserInterface()
    {
        _userInterface.AInitialize();
		_userInterface.mainCamera = _player.gameObject.GetComponentInChildren<Camera>();
		_player.refreshReloadUI +=_userInterface.onHudUpdateValues;
		_player.InitializeHudRefresher();
		
    }

	private void Update () 
    {
        _player.AUpdate();
		DataExchange();		
		

	}
	private void generateObjects(string p_receivedData)
	{

		char[] delimitersForObjects = { '/'};
		string[] objStrings = p_receivedData.Split(delimitersForObjects);
		
		for (int i=1; i< objStrings.Length; i++)
		{
			char[] __delimiterForInfo = { '|'};
			string[] __infoString = objStrings[i].Split(__delimiterForInfo);
			
			GameObject __go = findInList(int.Parse(__infoString[0]));
			if (__go == null)
			{
			//	Debug.Log("objeto serial " + __infoString[0] + " do tipo "+ __infoString[1] + " encontrado na posição " + __infoString[2]);	
				__go = Instantiate(prefabsForeignObjects[ int.Parse(__infoString[1])] );
				char __delimiterForVec = ',';
				string[] __vecString = __infoString[2].Split(__delimiterForVec);
				__go.transform.position = new Vector3(float.Parse(__vecString[0]),float.Parse(__vecString[1]),float.Parse(__vecString[2]));
				 __vecString = __infoString[3].Split(__delimiterForVec);
				__go.transform.eulerAngles = new Vector3(float.Parse(__vecString[0]),float.Parse(__vecString[1]),float.Parse(__vecString[2]));
				DataPacketServer __dp =  __go.AddComponent<DataPacketServer>();
				__dp.serial = int.Parse(__infoString[0]);
				__dp.type = int.Parse(__infoString[1]);
				_foreignDataPackets.Add(__dp);
			}
			else
			{
				char __delimiterForVec = ',';
				string[] __vecString = __infoString[2].Split(__delimiterForVec);
				__go.transform.position = new Vector3(float.Parse(__vecString[0]),float.Parse(__vecString[1]),float.Parse(__vecString[2]));
				 __vecString = __infoString[3].Split(__delimiterForVec);
				__go.transform.eulerAngles = new Vector3(float.Parse(__vecString[0]),float.Parse(__vecString[1]),float.Parse(__vecString[2]));	

			}

		}

	}
	private GameObject findInList (int serial)
	{
		GameObject result = null;
		foreach(DataPacketServer _dataPacket in _foreignDataPackets)	
			if (_dataPacket.serial == serial)
				result = _dataPacket.gameObject;

		return result;

	}
	private void DataExchange()
	{
		string streamString = "";
		streamString += _dataPackets.Count;
		foreach (DataPacketServer __dataPacket in _dataPackets)
		{
			
			streamString= streamString +"/"+__dataPacket.returnData();			

		}

		string __receivedData = "";
		if (connect)
		{
			__receivedData = _connectionScript.AUpdate(streamString);   
		
			if (__receivedData != "-1")
				 generateObjects(__receivedData);
		}


		

	}
}
