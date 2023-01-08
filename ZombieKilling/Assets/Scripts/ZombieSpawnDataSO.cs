using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSpawn.asset", menuName = "Scriptable Objects/Zombie Spawn Data")]
public class ZombieSpawnDataSO : ScriptableObject
{
    public GameObject ZombiePrefab;

    private Queue<ZombieSpawnPoint> _zombieSpawnPoints;

    public void ResetData()
    {
        _zombieSpawnPoints = null;
    }

    public void RegisterSpawnPoint(ZombieSpawnPoint point)
    {
        if(_zombieSpawnPoints == null) _zombieSpawnPoints= new Queue<ZombieSpawnPoint>();
        _zombieSpawnPoints.Enqueue(point);
    }

    public void RequestZombieSpawn()
    {
        ZombieSpawnPoint spawnPoint = _zombieSpawnPoints.Dequeue();
        spawnPoint.OnZombieSpawnRequest();
        _zombieSpawnPoints.Enqueue(spawnPoint);
    }
}
