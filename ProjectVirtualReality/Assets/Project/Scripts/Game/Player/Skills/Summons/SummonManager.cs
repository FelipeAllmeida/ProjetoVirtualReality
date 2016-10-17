using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SummonManager : MonoBehaviour
{
	public Action<DataPacketServer> onDestroy;
	public Action<DataPacketServer> onCreate;

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

    public void Summon(int p_serial, SummonsType p_summonType, Transform p_parent, Vector3 p_summonPosition, Quaternion p_quaternion)
    {
        GameObject __spawnedObject = SpawnerManager.SpawnAt(_dictSummonsPrefab[p_summonType], p_summonPosition, p_parent, p_quaternion);
		DataPacketServer __dataPacket = __spawnedObject.AddComponent<DataPacketServer>();
	
		onCreate(__dataPacket);
		__dataPacket.type = 0;
		__dataPacket.onDestroy += onDestroy;

    }
}
