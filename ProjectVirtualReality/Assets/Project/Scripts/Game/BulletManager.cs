using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour 
{
	public float propellentPower;
	public float damageCapacity;
	private Rigidbody _myRigidbody;
	private float _rotationSpeed;

	public void AInitialize()
	{
		_myRigidbody = transform.GetComponent<Rigidbody>();
		_myRigidbody.centerOfMass = new Vector3(_myRigidbody.centerOfMass.x, _myRigidbody.centerOfMass.y, _myRigidbody.centerOfMass.z - 1);
		_rotationSpeed = 6000;
		_myRigidbody.AddRelativeForce(0,0,propellentPower);
		

	}	
	public void Update()
	{
		//Yes, there are better ways to do that, but the object simply won't move fast enough with addtorque or angularVelocicy, don't know why
		if (_myRigidbody != null)
			_myRigidbody.transform.Rotate(0,0,_rotationSpeed*Time.deltaTime);

	}
	public void OnTriggerEnter(Collider other)
	{

		
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
