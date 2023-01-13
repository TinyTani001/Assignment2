using System.Collections;
using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    [SerializeField] private ZombieSpawnDataSO _zombieSpawnData;
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibelData;

    private GameObject _lastSpawnedZombie;
    private int _zombieSpawnRequestsCount, _collectedUpgradesCount;

    private void Start()
    {
        _zombieSpawnData.RegisterSpawnPoint(this);
        _upgradeCollectibelData.OnUpgradeCollected += (index, upgrade) => _collectedUpgradesCount++;
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
        if (_collectedUpgradesCount < 5)
        {
            _zombieSpawnRequestsCount--;
            _lastSpawnedZombie = Instantiate(_zombieSpawnData.ZombiePrefab, transform.position, Quaternion.identity);
            yield return new WaitUntil(() => _lastSpawnedZombie == null || (transform.position - _lastSpawnedZombie.transform.position).sqrMagnitude > 1f);
            _lastSpawnedZombie = null;
            if (_zombieSpawnRequestsCount > 0) StartCoroutine(HandleZombieSpawnRequest());
        }
    }
}
