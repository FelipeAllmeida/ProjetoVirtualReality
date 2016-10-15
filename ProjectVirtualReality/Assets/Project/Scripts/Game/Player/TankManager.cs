using UnityEngine;
using System.Collections;
using System;

public class TankManager : MonoBehaviour {
	
	private Transform _leftTrackPosition;	
	private Transform _rightTrackPosition;

	private Transform _turret;
	private float _turretRotationSpeed;
	private float _bodyBlockingRotationBehind;
	
	private int _health;
	private int _maxHealth;	

	private Transform _gun;
	private float _gunRotationSpeed;
	private float _gunMaxRotUp;
	private float _gunMaxRotDown;
	private int _ammo;

	private GameObject _ammunitionPrefab;

	private Rigidbody _myRigidbody;
	private float _engineForce;

	private float _currentReloadTime;
	[SerializeField]private float _reloadTime;

	public Action shootBullet;
	public Action<float> rotateTurret;
	public Action<float> rotateGun;
	public Action<Vector2> changeTracksState;

	public Action<GameStateTank.hudValues> refreshReloadUI;

	public enum EngineDistribution
	{
		Stopped=0,
		Quarter=1,Half=2,twoQuarter=3,Full=4,
	    QuarterReverse=-1,HalfReverse=-2,twoQuarterReverse=-3,FullReverse=-4

	}
	private EngineDistribution _leftTrackState;
	private EngineDistribution _rightTrackState;

	
	private void HandlerRotateTurret(float speed)
	{
		float tempGunRot = -_gun.localEulerAngles.x;
		if (tempGunRot > 180)
			tempGunRot -= 360;
		if (tempGunRot < -180)
			tempGunRot += 360;
		
		float finalRot = _turret.localEulerAngles.y+(speed*Time.deltaTime*_turretRotationSpeed);
		if (finalRot > 180)
			finalRot -= 360;
		if (finalRot < -180)
			finalRot += 360;

		if ( ((finalRot > _bodyBlockingRotationBehind)||(finalRot< -_bodyBlockingRotationBehind)) && (tempGunRot<0))
			finalRot = _turret.localEulerAngles.y;

		_turret.localEulerAngles = new Vector3(_turret.localEulerAngles.x,finalRot,_turret.localEulerAngles.z);

	}
	private void HandlerRotateGun(float speed)
	{
		float tempTurRot = _turret.localEulerAngles.y;
		if (tempTurRot > 180)
			tempTurRot -= 360;
		if (tempTurRot < -180)
			tempTurRot += 360;

		float finalRot = -_gun.localEulerAngles.x+(speed*Time.deltaTime*_gunRotationSpeed);

		if (finalRot > 180)
			finalRot -= 360;
		if (finalRot < -180)
			finalRot += 360;

		if (finalRot > _gunMaxRotUp)
			finalRot = _gunMaxRotUp;
		if (finalRot < _gunMaxRotDown)
			finalRot = _gunMaxRotDown;

		if ( ((tempTurRot > _bodyBlockingRotationBehind)||(tempTurRot< -_bodyBlockingRotationBehind)) && (finalRot<0))
			finalRot = _gun.localEulerAngles.x;

		_gun.localEulerAngles = new Vector3(-finalRot,_gun.localEulerAngles.y,_gun.localEulerAngles.z);

	}
	private void Reload()
	{
		if (_currentReloadTime > 0)
			_currentReloadTime -=Time.deltaTime;

	}

	private void HandlerShoot()
	{
	
		if (_currentReloadTime <= 0)
		{
			BulletManager _tempBulletManager = (Instantiate(_ammunitionPrefab,_gun.transform.position+ _gun.transform.TransformVector(new Vector3(0,0,3.5f)) ,_gun.transform.rotation) as GameObject).GetComponent<BulletManager>();
			_tempBulletManager.AInitialize();
			_myRigidbody.AddForceAtPosition(_gun.transform.TransformVector(0,0,-_tempBulletManager.propellentPower),_gun.transform.position+ _gun.transform.TransformVector(new Vector3(0,0,3.5f)));
			_currentReloadTime = _reloadTime;
			_ammo --;
		}
		

	}

	public void AInitialize(Transform leftTrackPosition, Transform rightTrackPosition, Rigidbody myRigidBody, Transform turret, Transform gun, GameObject ammunitionPrefab, float reloadTime) 
    {
		_leftTrackPosition = leftTrackPosition;
		_rightTrackPosition = rightTrackPosition;
		_leftTrackState = EngineDistribution.Stopped;
		_rightTrackState = EngineDistribution.Stopped;
		
		_myRigidbody = myRigidBody;
		_myRigidbody.centerOfMass = new Vector3(_myRigidbody.centerOfMass.x, _myRigidbody.centerOfMass.y-1,_myRigidbody.centerOfMass.z);
		
		_turret = turret;
		_turretRotationSpeed = 40;
		_bodyBlockingRotationBehind = 140;

		_gun = gun;
		_gunRotationSpeed = 20;
		_gunMaxRotDown = -15;
		_gunMaxRotUp = 30;

		_ammunitionPrefab = ammunitionPrefab;

		_health = 2;
		_maxHealth = 10;

		_ammo = 31;		

      	_engineForce = 15000;

		_reloadTime = reloadTime;

		shootBullet += HandlerShoot;
		rotateTurret += HandlerRotateTurret;
		rotateGun += HandlerRotateGun;
		changeTracksState += HandlerChangeTrackStats;

	}
	private void HandlerChangeTrackStats(Vector2 tracks)
	{

		_leftTrackState += Mathf.RoundToInt(tracks.x*2);
		_rightTrackState += Mathf.RoundToInt(tracks.y*2);

	}
	public void Aupdate()
	{
		MoveLeftTracks(_leftTrackState);
		MoveRightTracks(_rightTrackState);
		Reload();
		UpdateHud();		

	}
	private void UpdateHud()
	{
		GameStateTank.hudValues __hudValues = new GameStateTank.hudValues();

		__hudValues.reloadTime = 1-(_currentReloadTime / _reloadTime);
		__hudValues.turretRotation = _turret.localEulerAngles.y;
		__hudValues.rotation = _myRigidbody.transform.localEulerAngles.y;
		__hudValues.health = (float)_health / _maxHealth ;
		__hudValues.ammo = _ammo;
		
		if (refreshReloadUI != null);
			refreshReloadUI(__hudValues);

	}
	private void MoveLeftTracks(EngineDistribution _distribution)
	{
		
		Vector3 resultingForce = _leftTrackPosition.transform.TransformVector(new Vector3(0,0,_engineForce*(int)_distribution));

		Vector3 resultingPositionFront = _leftTrackPosition.transform.position + _leftTrackPosition.transform.TransformVector(new Vector3(-0.2f,0,3.5f));
		Vector3 resultingPositionBack = _leftTrackPosition.transform.position + _leftTrackPosition.transform.TransformVector(new Vector3(-0.2f,0,-3.5f));

		Quaternion rotation = Quaternion.Euler(0,8,0);
		Vector3 resultingForceFront = rotation*resultingForce;
		rotation = Quaternion.Euler(0,-8,0);
		Vector3 resultingForceBack = rotation*resultingForce;
		
		_myRigidbody.AddForceAtPosition(resultingForceFront,resultingPositionFront);
		_myRigidbody.AddForceAtPosition(resultingForceBack,resultingPositionBack);

		_leftTrackState=0;	
					
	}
	private void MoveRightTracks(EngineDistribution _distribution)
	{
		Vector3 resultingForce = _rightTrackPosition.transform.TransformVector(new Vector3(0,0,_engineForce*(int)_distribution));

		Vector3 resultingPositionFront = _rightTrackPosition.transform.position + _rightTrackPosition.transform.TransformVector(new Vector3(0.2f,0,3.5f));
		Vector3 resultingPositionBack = _rightTrackPosition.transform.position + _rightTrackPosition.transform.TransformVector(new Vector3(0.2f,0,-3.5f));

		Quaternion rotation = Quaternion.Euler(0,-8,0);
		Vector3 resultingForceFront = rotation*resultingForce;
		rotation = Quaternion.Euler(0,8,0);
		Vector3 resultingForceBack = rotation*resultingForce;

		_myRigidbody.AddForceAtPosition(resultingForceFront,resultingPositionFront);
		_myRigidbody.AddForceAtPosition(resultingForceBack,resultingPositionBack);
		
		_rightTrackState=0;		
	}
	
}
