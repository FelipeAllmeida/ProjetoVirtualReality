using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
    public static GameObject SpawnAt(GameObject p_gameObject, Vector3 p_spawnPosition, Transform p_parent, Quaternion p_quaternion)
    {
        GameObject __spawnedGameObject = GameObject.Instantiate(p_gameObject, p_spawnPosition, p_quaternion, p_parent) as GameObject;

        return __spawnedGameObject;
    }
    
}
