﻿using UnityEngine;
using System.Collections;

public class TankManager : MonoBehaviour {
	
	private Transform _leftTrackPosition;	
	private Transform _rightTrackPosition;

	private Transform _turret;
	private float _turretRotationSpeed;
	private float _bodyBlockingRotationBehind;
	
	private int _health;

	private Transform _gun;
	private float _gunRotationSpeed;
	private float _gunMaxRotUp;
	private float _gunMaxRotDown;

	private GameObject _ammunitionPrefab;

	private Rigidbody _myRigidbody;
	private float _engineForce;

	public enum EngineDistribution
	{
		Stopped=0,
		Quarter=1,Half=2,twoQuarter=3,Full=4,
	    QuarterReverse=-1,HalfReverse=-2,twoQuarterReverse=-3,FullReverse=-4

	}
	private EngineDistribution _leftTrackState;
	private EngineDistribution _rightTrackState;

	
	public void RotateTurret(float speed)
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
	public void RotateGun(float speed)
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

	public void ShootGun()
	{
	
		BulletManager _tempBulletManager = (Instantiate(_ammunitionPrefab,_gun.transform.position+ _gun.transform.TransformVector(new Vector3(0,0,3.5f)) ,_gun.transform.rotation) as GameObject).GetComponent<BulletManager>();
		_tempBulletManager.AInitialize();
		

	}

	public void AInitialize(Transform leftTrackPosition, Transform rightTrackPosition, Rigidbody myRigidBody, Transform turret, Transform gun, GameObject ammunitionPrefab) 
    {
		_leftTrackPosition = leftTrackPosition;
		_rightTrackPosition = rightTrackPosition;
		_leftTrackState = EngineDistribution.Stopped;
		_rightTrackState = EngineDistribution.Stopped;
		
		_myRigidbody = myRigidBody;
		_myRigidbody.centerOfMass = new Vector3(_myRigidbody.centerOfMass.x, _myRigidbody.centerOfMass.y-1,_myRigidbody.centerOfMass.z);
		
		_turret = turret;
		_turretRotationSpeed = 20;
		_bodyBlockingRotationBehind = 140;

		_gun = gun;
		_gunRotationSpeed = 20;
		_gunMaxRotDown = -15;
		_gunMaxRotUp = 30;

		_ammunitionPrefab = ammunitionPrefab;

		_health = 10;
		
      	_engineForce = 15000;

	}
	public void ChangeTrackStats(float leftTrack, float rightTrack)
	{

		_leftTrackState += Mathf.RoundToInt(leftTrack*2);
		_rightTrackState += Mathf.RoundToInt(rightTrack*2);

	}
	public void Aupdate()
	{
		MoveLeftTracks(_leftTrackState);
		MoveRightTracks(_rightTrackState);
		_rightTrackState=0;
		_leftTrackState=0;		

	}
	private void MoveLeftTracks(EngineDistribution _distribution)
	{
		
		Vector3 resultingForce = _leftTrackPosition.transform.TransformVector(new Vector3(0,0,_engineForce*(int)_distribution));

		Vector3 resultingPositionFront = _leftTrackPosition.transform.position + _leftTrackPosition.transform.TransformVector(new Vector3(-0.2f,0,3.5f));
		Vector3 resultingPositionBack = _leftTrackPosition.transform.position + _leftTrackPosition.transform.TransformVector(new Vector3(-0.2f,0,-3.5f));

		Quaternion rotation = Quaternion.Euler(0,15,0);
		Vector3 resultingForceFront = rotation*resultingForce;
		rotation = Quaternion.Euler(0,-15,0);
		Vector3 resultingForceBack = rotation*resultingForce;
		
		_myRigidbody.AddForceAtPosition(resultingForceFront,resultingPositionFront);
		_myRigidbody.AddForceAtPosition(resultingForceBack,resultingPositionBack);
					
	}
	private void MoveRightTracks(EngineDistribution _distribution)
	{
		Vector3 resultingForce = _rightTrackPosition.transform.TransformVector(new Vector3(0,0,_engineForce*(int)_distribution));

		Vector3 resultingPositionFront = _rightTrackPosition.transform.position + _rightTrackPosition.transform.TransformVector(new Vector3(0.2f,0,3.5f));
		Vector3 resultingPositionBack = _rightTrackPosition.transform.position + _rightTrackPosition.transform.TransformVector(new Vector3(0.2f,0,-3.5f));

		Quaternion rotation = Quaternion.Euler(0,-15,0);
		Vector3 resultingForceFront = rotation*resultingForce;
		rotation = Quaternion.Euler(0,15,0);
		Vector3 resultingForceBack = rotation*resultingForce;

		_myRigidbody.AddForceAtPosition(resultingForceFront,resultingPositionFront);
		_myRigidbody.AddForceAtPosition(resultingForceBack,resultingPositionBack);
				
	}
	
}