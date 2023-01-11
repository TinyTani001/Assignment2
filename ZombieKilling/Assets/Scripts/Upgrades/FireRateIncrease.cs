using UnityEngine;

[CreateAssetMenu(fileName = "FireRateIncrease.asset", menuName = "Scriptable Objects/Upgrades/Fire Rate Increase")]
public class FireRateIncrease : UpgradeBase
{
    [SerializeField] private float _fireRate;
    [SerializeField] private PlayerDataSO _playerData;
    public override void ActivateUpgrade()
    {
        _playerData.SetFireRate(_fireRate);
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
