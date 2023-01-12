using UnityEngine;

[CreateAssetMenu(fileName = "InvisiblePlayer.asset", menuName = "Scriptable Objects/Upgrades/Invisible Player")]
public class InvisiblePlayer : UpgradeBase
{
    [SerializeField] private PlayerDataSO _playerData;
    public override void ActivateUpgrade()
    {
        _playerData.MakePlayerInvisible();
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
