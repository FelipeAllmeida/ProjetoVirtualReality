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

    private float _fireRate = 1f;
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

    private IEnumerator ShootTarget()
    {
        while (_currentState == StateType.SHOOTING)
        {
            GameObject __bulletInstance = SpawnerManager.SpawnAt(_bulletPrefab, transform.position + transform.forward, transform, transform.rotation);
            __bulletInstance.GetComponent<Bullet>().Ainitialize();
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
