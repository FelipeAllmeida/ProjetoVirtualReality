using UnityEngine;
using System;
using System.Collections;

public class Turret : MonoBehaviour 
{
    #region Enumerators
    private enum StateType
    {
        SHOOTING,
        ON_HOLD
    }
    #endregion
    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;

    #region Private Data
    private GameObject _target;

    [SerializeField] private StateType _currentState;

    private float _fireRate = 3f;
	private float timerToShoot = 3f;
    #endregion

	public Action<DataPacketServer> onCreateBullet;
	public Action<DataPacketServer> onDestroyBullet;
	private GameObject bulletTargetTransform;

    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target.transform, new Vector3(0, 1, 0));
			if (bulletTargetTransform == null)
				bulletTargetTransform = new GameObject();
			bulletTargetTransform.transform.position = transform.position;
			bulletTargetTransform.transform.LookAt(new Vector3(_target.transform.position.x,_target.transform.position.y-1,_target.transform.position.z));
			if (timerToShoot <= 0)
				ShootTarget();
        }
		if (timerToShoot > 0)
		timerToShoot -= Time.deltaTime;	
		
    }

    private void SwitchState(StateType p_stateType)
    {
        _currentState = p_stateType;
   
    }
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "TankBullet" )
			TakeDamage();		

	}
	private void TakeDamage()
	{

		gameObject.GetComponent<DataPacketServer>().HandlerDestroy();
		Destroy(gameObject);

	}

    private void ShootTarget()
    {
        
            Vector3 __bulletSpawnPosition = new Vector3(transform.position.x, 3.5f, transform.position.z);
            GameObject __bulletInstance = SpawnerManager.SpawnAt(_bulletPrefab, __bulletSpawnPosition + transform.forward, null, bulletTargetTransform.transform.rotation);
            __bulletInstance.GetComponent<BulletManager>().AInitialize();
			DataPacketServer __DP = __bulletInstance.gameObject.AddComponent<DataPacketServer>();
			__DP.position = __bulletInstance.transform.position;
			__DP.rotation = __bulletInstance.transform.eulerAngles;
			__DP.type = 1;
			__DP.onDestroy += onDestroyBullet;
			onCreateBullet(__DP);
			timerToShoot = _fireRate;
    
        
    }

    void OnTriggerEnter(Collider p_collider)
    {	
		Debug.Log(tag);
        if (p_collider != null)
        {
            if (p_collider.gameObject.tag == "Player")
            {
                _target = p_collider.gameObject;
                SwitchState(StateType.SHOOTING);     
				
            }        
        }
    }

    void OnTriggerExit(Collider p_collider)
    {
        if (_target != null)
        {
            if (_target == p_collider.gameObject)
            {
                _target = null;
                SwitchState(StateType.ON_HOLD);
            }
        }
    }
}
