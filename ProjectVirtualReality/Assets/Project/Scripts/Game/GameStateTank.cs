
using UnityEngine;
using System.Collections;

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
    #endregion

    #region Private Data
    private TankPlayer _player;
	private ConnectionScript _connectionScript;
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
		if (connect)
			_connectionScript.AUpdate("Sou o client");   
		

	}
}
