using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{
    Player _player;
	private void Start () 
    {
        InitializePlayer();
	}

    private void InitializePlayer()
    {
        _player = new Player();
        _player.AInitialize();
    }

	private void Update () 
    {
        _player.AUpdate();
	}
}
