using UnityEngine;

public class UpgradeCollectibeManager : MonoBehaviour
{
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;
    [SerializeField] private GameObject _upgradeCollectiblePrefab;
    [SerializeField] private ZombieSpawnDataSO _zombieSpawnData;

    private int _currentZombieKillsToUpgrade;

    private void Awake()
    {
        _upgradeCollectibeData.AssignResources();
    }

    private void Start()
    {
        _zombieSpawnData.OnZombieSpawnRequest += deathPosition =>
        {
            if (_upgradeCollectibeData.CanDropUpgradeCollectible)
            {
                _currentZombieKillsToUpgrade++;
                _upgradeCollectibeData.OnZombieKillPercentUpdated?.Invoke(Mathf.InverseLerp(0f, _upgradeCollectibeData.ZombieKillsToUpgrade, _currentZombieKillsToUpgrade));
                if (_currentZombieKillsToUpgrade == _upgradeCollectibeData.ZombieKillsToUpgrade)
                {
                    int index = _upgradeCollectibeData.GetNewCollectibleIndex();
                    _currentZombieKillsToUpgrade = 0;
                    UpgradeCollectible collectible = Instantiate(_upgradeCollectiblePrefab, deathPosition + Vector3.up * 0.1f, Quaternion.identity).GetComponent<UpgradeCollectible>();
                    collectible.SetUpgradeIndex(index);
                }
            }
        };
    }

    private void OnDestroy()
    {
        _upgradeCollectibeData.ClearResources();
    }
}
