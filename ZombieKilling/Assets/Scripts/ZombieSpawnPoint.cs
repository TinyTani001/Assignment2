using System.Collections;
using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    [SerializeField] private ZombieSpawnDataSO _zombieSpawnData;

    private GameObject _lastSpawnedZombie;
    private int _zombieSpawnRequestsCount;

    private void Start()
    {
        _zombieSpawnData.RegisterSpawnPoint(this);
    }

    private void OnDestroy()
    {
        _zombieSpawnData.ResetData();
    }

    public void OnZombieSpawnRequest()
    {
        _zombieSpawnRequestsCount++;
        if(_lastSpawnedZombie == null)
        {
            StartCoroutine(HandleZombieSpawnRequest());
        }
    }

    IEnumerator HandleZombieSpawnRequest()
    {
        _zombieSpawnRequestsCount--;
        _lastSpawnedZombie = Instantiate(_zombieSpawnData.ZombiePrefab, transform.position, Quaternion.identity);
        yield return new WaitUntil(()=> _lastSpawnedZombie == null || (transform.position - _lastSpawnedZombie.transform.position).sqrMagnitude > 1f);
        _lastSpawnedZombie = null;
        if(_zombieSpawnRequestsCount > 0) StartCoroutine(HandleZombieSpawnRequest());
    }
}
