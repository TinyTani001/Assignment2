using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCollectibe.asset", menuName = "Scriptable Objects/Upgrade Collectible Data")]
public class UpgradeCollectibleDataSO : ScriptableObject
{
    [SerializeField] private int _zombieKillsToUpgrade;
    [SerializeField] private UpgradeBase[] _upgradeList;

    public Action<int, UpgradeBase> OnUpgradeCollected;
    public Action<float> OnZombieKillPercentUpdated;

    public int ZombieKillsToUpgrade => _zombieKillsToUpgrade;

    public bool CanDropUpgradeCollectible => _remainingUpgradesToCollect.Count> 0;

    [NonSerialized] private UpgradeBase[] _collectedUpgrades;
    [NonSerialized] private List<int> _remainingUpgradesToCollect;
    [NonSerialized] private UpgradeBase _lastActiveUpgrade;

    public void UpgradeCollected()
    {
        int max = _remainingUpgradesToCollect.Count;
        if (max > 0)
        {
            int index = UnityEngine.Random.Range(0, max);
            UpgradeBase upgradeCollected = _upgradeList[_remainingUpgradesToCollect[index]];
            _collectedUpgrades[_remainingUpgradesToCollect[index]] = upgradeCollected;
            OnUpgradeCollected?.Invoke(index, upgradeCollected);
            _remainingUpgradesToCollect.RemoveAt(index);
        }
    }

    public void ActivateUpgradeOnIndex(int index)
    {
        if(_lastActiveUpgrade != null) _lastActiveUpgrade.DeactivateUpgrade();
        _lastActiveUpgrade = _collectedUpgrades[index];
        if(_lastActiveUpgrade != null) _lastActiveUpgrade.ActivateUpgrade();
    }

    public void AssignResources()
    {
        int length = _upgradeList.Length;
        _collectedUpgrades = new UpgradeBase[length];
        _remainingUpgradesToCollect = new List<int>();
        for (int i = 0; i < length; i++)
        {
            _remainingUpgradesToCollect.Add(i);
        }
    }

    public void ClearResources()
    {
        _lastActiveUpgrade = null;
        OnUpgradeCollected = null;
        _remainingUpgradesToCollect = null;
        _collectedUpgrades = null;
    }
}
