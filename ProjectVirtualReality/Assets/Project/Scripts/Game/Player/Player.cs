using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private InputManager _inputManager;

	void AInitialize() 
    {
        InitializePlayer();
	}

    private void InitializePlayer()
    {
        _inputManager = new InputManager();
    }

    public void AUpdate()
    {
        _inputManager.AUpdate();
    }
}
