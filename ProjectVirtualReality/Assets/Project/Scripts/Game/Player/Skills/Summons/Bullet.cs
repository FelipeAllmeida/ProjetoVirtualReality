using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    #region Serialized Fields
    [Header("Bullet Config")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    #endregion

    public void Ainitialize()
    {
        Destroy(gameObject, _lifeTime);
        Rigidbody __rigidbody = gameObject.GetComponent<Rigidbody>();
        __rigidbody.velocity = transform.forward * _speed;
    }

    public void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }

    private void OnCollisionEnter(Collision p_collision)
    {
        if (p_collision.gameObject.tag == "Player")
        {
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
