using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotKill.asset", menuName = "Scriptable Objects/Upgrades/Single Shot Kill")]
public class SingleShotKill : UpgradeBase
{
    [SerializeField] private PlayerDataSO _playerData;
    public override void ActivateUpgrade()
    {
        _playerData.SetBulletDamage(999);
        _playerData.SetFireRate(1);
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
