using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSpawn.asset", menuName = "Scriptable Objects/Zombie Spawn Data")]
public class ZombieSpawnDataSO : ScriptableObject
{
    public GameObject ZombiePrefab;

    public Action<Vector3> OnZombieSpawnRequest;
    public Action<int> OnZombieCountUpdated;

    private Queue<ZombieSpawnPoint> _zombieSpawnPoints;

    public int RemainingZombiesCount
    {
        get => _remainingZombiesCount;
        set
        {
            _remainingZombiesCount = value;
            OnZombieCountUpdated?.Invoke(_remainingZombiesCount);
        }
    }

    private int _remainingZombiesCount;

    public void ResetData()
    {
        _zombieSpawnPoints = null;
        OnZombieSpawnRequest = null;
        _remainingZombiesCount = 0;
    }

    public void RegisterSpawnPoint(ZombieSpawnPoint point)
    {
        if (_zombieSpawnPoints == null) _zombieSpawnPoints = new Queue<ZombieSpawnPoint>();
        _zombieSpawnPoints.Enqueue(point);
    }

    public void RequestZombieSpawn(Vector3 deathPosition)
    {
        OnZombieSpawnRequest?.Invoke(deathPosition);
        ZombieSpawnPoint spawnPoint = _zombieSpawnPoints.Dequeue();
        spawnPoint.OnZombieSpawnRequest();
        _zombieSpawnPoints.Enqueue(spawnPoint);
    }
}
