using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour 
{
    #region Serializable Data
    [Header("Transforms")]
    [SerializeField] private Transform _static;
    [SerializeField] private Transform _dynamic;
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabPlayer;
    [Header("User Interface")]
    [SerializeField] private UI _userInterface;
	[Header("Connection")]
	[SerializeField] private bool connect;
	[SerializeField] private string _ip;
	[SerializeField] private int _port;
    #endregion

    #region Private Data
    private Player _player;
	private ConnectionScript _connectionScript;
	private List<DataPacketServer> _dataPackets;
	private List<DataPacketServer> _foreignDataPackets;
	[SerializeField]private GameObject[] prefabsForeignObjects;
	private int serialData;
    #endregion

    private void Start () 
    {
		_dataPackets = new List<DataPacketServer>();	
_foreignDataPackets = new List<DataPacketServer>();	
		serialData = 0;
        InitializePlayer();
        InitializeUserInterface();
		if (connect)
			InitializeConnection();
        ListenUIEvents();

    }
	private void InitializeConnection()
	{

		_connectionScript = transform.gameObject.AddComponent<ConnectionScript>();
		_connectionScript.AInitialize(true,_ip,_port);


	}
    private void ListenUIEvents()
    {
        _userInterface.onClickPlayerSkillButton += delegate (PlayerSkills.SkillType p_skillType)
        {
            _player.HandleSkillClick(serialData, p_skillType);
			serialData++;
        };
    }

    private void InitializePlayer()
    {
        GameObject __playerGameObject = SpawnerManager.SpawnAt(_prefabPlayer, new Vector3(0f, 0f, 0f), _dynamic, new Quaternion(0f, 0f, 0f, 0f));
        _player = __playerGameObject.GetComponent<Player>();
        _player.AInitialize();  
    }

    private void InitializeUserInterface()
    {
        _userInterface.AInitialize();
    }

	private void Update () 
    {
        _player.AUpdate();
        _userInterface.AUpdate();
		_dataPackets =  _player.getListOfPackets();
		DataExchange();
	}
	private void GenerateObjects(string p_receivedData)
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
				 GenerateObjects(__receivedData);
		}
	}
}
