using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCollectibe.asset", menuName = "Scriptable Objects/Upgrade Collectible Data")]
public class UpgradeCollectibleDataSO : ScriptableObject
{
    [SerializeField] private int _zombieKillsToUpgrade;

    public Action OnUpgradeCollected;
    public Action<float> OnZombieKillPercentUpdated;

    public int ZombieKillsToUpgrade => _zombieKillsToUpgrade;

    public void UpgradeCollected()
    {
        OnUpgradeCollected?.Invoke();
    }

    public void ClearResources()
    {
        OnUpgradeCollected = null;
    }
}
