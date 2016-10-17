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
    private Coroutine _shootingCoroutine;
    #endregion

    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target.transform, new Vector3(0, 1, 0));
        }
    }

    private void SwitchState(StateType p_stateType)
    {
        _currentState = p_stateType;
        switch (_currentState)
        {
            case StateType.SHOOTING:
                _shootingCoroutine = StartCoroutine(ShootTarget());
                break;
            case StateType.ON_HOLD:
                StopCoroutine(_shootingCoroutine);
                break;
        }
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

    private IEnumerator ShootTarget()
    {
        while (_currentState == StateType.SHOOTING)
        {
            Vector3 __bulletSpawnPosition = new Vector3(transform.position.x, 3.5f, transform.position.z);
            GameObject __bulletInstance = SpawnerManager.SpawnAt(_bulletPrefab, __bulletSpawnPosition + transform.forward, null, transform.rotation);
            __bulletInstance.GetComponent<BulletManager>().AInitialize();
            yield return new WaitForSeconds(_fireRate);        
        }
    }

    void OnTriggerEnter(Collider p_collider)
    {
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
