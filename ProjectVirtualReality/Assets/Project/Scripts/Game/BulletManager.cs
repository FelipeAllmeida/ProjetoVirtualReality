using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour 
{
	public float propellentPower;
	public float damageCapacity;
	private Rigidbody _myRigidbody;
	private float _rotationSpeed;
	private GameObject _bulletCam;
	private float timerBullet;

	public void AInitialize()
	{
		_myRigidbody = transform.GetComponent<Rigidbody>();
		_myRigidbody.centerOfMass = new Vector3(_myRigidbody.centerOfMass.x, _myRigidbody.centerOfMass.y, _myRigidbody.centerOfMass.z - 1);
		_rotationSpeed = 6000;
		_myRigidbody.AddRelativeForce(0,0,propellentPower);
		_bulletCam = new GameObject();
		Camera __cam =  _bulletCam.AddComponent<Camera>();
		__cam.rect = new Rect(0.8f,0.7f,0.2f,0.3f);
		timerBullet = 5;
		

	}	
	public void Update()
	{
		//Yes, there are better ways to do that, but the object simply won't move fast enough with addtorque or angularVelocicy, don't know why
		if (_myRigidbody != null)
			_myRigidbody.transform.Rotate(0,0,_rotationSpeed*Time.deltaTime);

		Vector3 relativePosition = new Vector3(0,0,-1);

		if (_bulletCam != null)
		{
			_bulletCam.transform.position = transform.position + transform.TransformDirection(relativePosition);
			_bulletCam.transform.position = _bulletCam.transform.position + new Vector3(0,1,0);
		
			_bulletCam.transform.LookAt(transform);

		}
		if (timerBullet > 0)
			timerBullet -= Time.deltaTime;
		else
		{
			setToDestroy();
			Destroy(_bulletCam);
		}

	}
	public void OnTriggerEnter(Collider other)
	{

		Destroy(_bulletCam);
		setToDestroy();
		

	}
	public void setToDestroy()
	{

		foreach(Collider tempCollider in 	transform.GetComponentsInChildren<Collider>())
			tempCollider.enabled = false;

		foreach(MeshRenderer tempRenderer in transform.GetComponentsInChildren<MeshRenderer>())
			tempRenderer.enabled = false;

		_myRigidbody.velocity = Vector3.zero;
		_myRigidbody.angularVelocity = Vector3.zero;


		Invoke("DestroyBullet",2);

	}
	public void DestroyBullet()
	{
		
		Destroy(this.gameObject);

	}

}
