
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
	private List<int> _dataPacketsToDestroy;
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
		_dataPacketsToDestroy = new List<int>();
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

	private void DestroyObject(DataPacketServer p_DP)
	{

		_dataPackets.Remove(p_DP);
		Destroy(p_DP.gameObject);		

	}
	private void CreateObject(DataPacketServer p_DP)
	{

		p_DP.serial = serialData;
		serialData++;
		_dataPackets.Add(p_DP);
		p_DP.gameObject.transform.parent = _dynamic;

	}

    private void InitializePlayer()
    {
        GameObject __playerGameObject = SpawnerManager.SpawnAt(_prefabPlayer, new Vector3(0f, 0f, 0f), _dynamic, new Quaternion(0f, 0f, 0f, 0f));
        _player = __playerGameObject.GetComponent<TankPlayer>();
		_player.onCreateBullet += CreateObject;
		_player.onDestroyBullet += DestroyObject;


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
	private void DestroyObject(int serial)
	{
		DataPacketServer result = null;
		foreach(DataPacketServer _dataPacket in _dataPackets)	
			if (_dataPacket.serial == serial)
				result = _dataPacket;

		_dataPackets.Remove(result);
		Destroy(result.gameObject);		

	}
	private void GenerateObjects(string p_receivedData)
	{
		
		_dataPacketsToDestroy = new List<int>();
		char[] delimitersForObjects = { '/'};
		string[] objStrings = p_receivedData.Trim().Split(delimitersForObjects);
		
		for (int i=1; i< objStrings.Length; i++)
		{
			char[] __delimiterForInfo = { '|','/'};
			string[] __infoString = objStrings[i].Split(__delimiterForInfo);

			

			int x = -1;
			int.TryParse(__infoString[0],out x);
			if (x >= 0)
			{
				_dataPacketsToDestroy.Add(x);
				GameObject __go = findInList(x);
				if (__go == null)
				{
					
					__go = Instantiate(prefabsForeignObjects[ int.Parse(__infoString[1])] );
				
					char __delimiterForVec = ',';
					string[] __vecString = __infoString[2].Split(__delimiterForVec);
					__go.transform.position = new Vector3(float.Parse(__vecString[0].Trim()),float.Parse(__vecString[1].Trim()),float.Parse(__vecString[2].Trim()));
					 __vecString = __infoString[3].Split(__delimiterForVec);
					__go.transform.eulerAngles = new Vector3(float.Parse(__vecString[0].Trim()),float.Parse(__vecString[1].Trim()),float.Parse(__vecString[2].Trim()));
					DataPacketServer __dp =  __go.AddComponent<DataPacketServer>();
					__dp.serial = x;
					_foreignDataPackets.Add(__dp);
					
				}
				else
				{
					char __delimiterForVec = ',';
					string[] __vecString = __infoString[2].Split(__delimiterForVec);
					__go.transform.position = new Vector3(float.Parse(__vecString[0].Trim()),float.Parse(__vecString[1].Trim()),float.Parse(__vecString[2].Trim()));
					 __vecString = __infoString[3].Split(__delimiterForVec);
					__go.transform.eulerAngles = new Vector3(float.Parse(__vecString[0].Trim()),float.Parse(__vecString[1].Trim()),float.Parse(__vecString[2].Trim()));	
	
				}
			}
		}
		cleanList();
	}
	private void cleanList()
	{

		//apaga os objetos que não estão mais no outro lado

		List<DataPacketServer> __DPToRemove = new List<DataPacketServer>();
		foreach (DataPacketServer __DP in _foreignDataPackets)
		{
			bool found = false;
			foreach (int x in _dataPacketsToDestroy)
			{
				if (__DP.serial == x)
					found = true;
			}
			if (!found)
				__DPToRemove.Add(__DP);




		}

		foreach (DataPacketServer __DP in __DPToRemove)
		{
			_foreignDataPackets.Remove(__DP);
			Destroy(__DP.gameObject);
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
				 GenerateObjects(__receivedData);
		}


		

	}
}
