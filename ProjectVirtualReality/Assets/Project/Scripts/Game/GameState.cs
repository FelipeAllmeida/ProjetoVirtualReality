using UnityEngine;
using System.Collections;

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
    #endregion

    private void Start () 
    {
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

	private void Update () 
    {
        _player.AUpdate();
        _userInterface.AUpdate();
		if (connect)
			_connectionScript.AUpdate("Sou o servidor");
	}
}
