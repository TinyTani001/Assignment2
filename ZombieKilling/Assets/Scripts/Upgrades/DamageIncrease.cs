using UnityEngine;

[CreateAssetMenu(fileName = "DamageIncrease.asset", menuName = "Scriptable Objects/Upgrades/Damage Increase")]
public class DamageIncrease : UpgradeBase
{
    [SerializeField] private int _damage;
    [SerializeField] private PlayerDataSO _playerData;
    public override void ActivateUpgrade()
    {
        _playerData.SetBulletDamage(_damage);
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
