using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummonManager : MonoBehaviour
{
    public enum SummonsType
    {
        TURRET
    }

    public float summonsLifeTime;

    [SerializeField] private GameObject _predfabTurret;

    private Dictionary<SummonsType, GameObject> _dictSummonsPrefab;

    private SummonManager _summonManager;

    public void Initialize()
    {
        InitializeSummonsDictionary();
    }

    private void InitializeSummonsDictionary()
    {
        _dictSummonsPrefab = new Dictionary<SummonsType, GameObject>();

        _dictSummonsPrefab.Add(SummonsType.TURRET, _predfabTurret);
    }

    public DataPacketServer Summon(int p_serial, SummonsType p_summonType, Transform p_parent, Vector3 p_summonPosition, Quaternion p_quaternion)
    {
        GameObject __spawnedObject = SpawnerManager.SpawnAt(_dictSummonsPrefab[p_summonType], p_summonPosition, p_parent, p_quaternion);
		DataPacketServer __dataPacket = __spawnedObject.AddComponent<DataPacketServer>();
		__dataPacket.serial = p_serial;
		
		return __dataPacket;

    }
}
