using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            _player.HandleSkillClick(p_skillType);
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
	private void GenerateObjects(string p_receivedData)
	{
		
			

	}

	private void Update () 
    {
        _player.AUpdate();
        _userInterface.AUpdate();
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
			
			Debug.Log("objeto serial " + __infoString[0] + " do tipo "+ __infoString[1] + " encontrado na posição " + __infoString[2]);			
		


		}

	}
	private bool findInList (int serial)
	{
		bool result = false;
		foreach(DataPacketServer _dataPacket in _foreignDataPackets)	
			if (_dataPacket.serial == serial)
				result = true;

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
			Debug.Log(__receivedData);
			if (__receivedData != "-1")
				 generateObjects(__receivedData);
		}


		

	}
}
