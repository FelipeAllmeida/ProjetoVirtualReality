
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
    [SerializeField] private UI _userInterface;
    #endregion

    #region Private Data
    private TankPlayer _player;
    #endregion

    private void Start () 
    {
        InitializePlayer();
  //      InitializeUserInterface();

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
    }

	private void Update () 
    {
        _player.AUpdate();
//        _userInterface.AUpdate();
	}
}
