using UnityEngine;

[CreateAssetMenu(fileName = "DamageIncrease.asset", menuName = "Scriptable Objects/Upgrades/Damage Increase")]
public class DamageIncrease : UpgradeBase
{
    [SerializeField] private int _damage;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private AudioClip _bulletSoundClip;
    public override void ActivateUpgrade()
    {
        _playerData.SetBulletDamage(_damage);
        _playerData.SetBulletSound(_bulletSoundClip);
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
