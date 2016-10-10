using UnityEngine;
using System.Collections;

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
	private void InitializeTank()
	{
		_tankManager = new TankManager();
		_tankManager.AInitialize(_leftTrack, _rightTrack, transform.GetComponent<Rigidbody>(),_turret,_gun,_ammunitionPrefab);

	}
    private void InitializeInputManager()
    {
        _inputManager = new InputManagerTank();
        ListenInputManagerEvents();
    }


    private void ListenInputManagerEvents()
    {
		
		_inputManager.onPressHorizontalAxis += _tankManager.ChangeTrackStats;     
		_inputManager.onPressVerticalAxis += _tankManager.ChangeTrackStats;   
       
		_inputManager.onPressTurretRotate += _tankManager.RotateTurret;
		_inputManager.onPressGunRotate += _tankManager.RotateGun;

		_inputManager.onPressFire += delegate {
			
			_tankManager.ShootGun();

		};

    }


    public void AUpdate()
    {
    	_inputManager.AUpdate();
		_tankManager.Aupdate();
    }

   

    
}