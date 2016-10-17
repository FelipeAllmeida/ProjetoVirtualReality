using UnityEngine;
using System.Collections;
using System;


public class TankPlayer : MonoBehaviour 
{
    private CameraManager _cameraManager;
	private TankManager _tankManager;
    private InputManagerTank _inputManager;
	public Transform _leftTrack;
	public Transform _rightTrack;
	public Transform _turret;
	public Transform _gun;
	public GameObject _ammunitionPrefab;

	[SerializeField] private float reloadTime;
	
	public Action<GameStateTank.hudValues> refreshReloadUI;
	public Action<DataPacketServer> onCreateBullet;
	public Action<DataPacketServer> onDestroyBullet;

	public void AInitialize() 
    {
	

        InitializeCameraManager();
        InitializePlayer();
		
	}

    private void InitializeCameraManager()
    {
        _cameraManager = new CameraManager();
        _cameraManager.AInitialize();
    }

    private void InitializePlayer()
    {
		InitializeTank();
        InitializeInputManager();	
  
    }
	
	public void InitializeHudRefresher()
	{
		_tankManager.refreshReloadUI += refreshReloadUI;					

	}
	private void InitializeTank()
	{
		_tankManager = new TankManager();
		_tankManager.onCreateBullet += onCreateBullet;
		_tankManager.onDestroyBullet += onDestroyBullet;
		_tankManager.AInitialize(_leftTrack, _rightTrack, transform.GetComponent<Rigidbody>(),_turret,_gun,_ammunitionPrefab,reloadTime);
		

	}
    private void InitializeInputManager()
    {
        _inputManager = new InputManagerTank();
        ListenInputManagerEvents();
    }
    private void ListenInputManagerEvents()
    {
		
		_inputManager.onPressHorizontalAxis += _tankManager.changeTracksState;     
		_inputManager.onPressVerticalAxis += _tankManager.changeTracksState;   
       
		_inputManager.onPressTurretRotate += _tankManager.rotateTurret;
		_inputManager.onPressGunRotate += _tankManager.rotateGun;

		_inputManager.onPressFire += _tankManager.shootBullet;

    }

    public void AUpdate()
    {
    	_inputManager.AUpdate();
		_tankManager.Aupdate();
		
    }
    
}