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
    #endregion

    #region Private Data
    private Player _player;
    #endregion

    private void Start () 
    {
        InitializePlayer();
        InitializeUserInterface();

        ListenUIEvents();

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
	}
}
