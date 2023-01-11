using UnityEngine;

[CreateAssetMenu(fileName = "InvisiblePlayer.asset", menuName = "Scriptable Objects/Upgrades/Invisible Player")]
public class InvisiblePlayer : UpgradeBase
{
    public override void ActivateUpgrade()
    {
        Debug.Log("Invisible Player activated");
    }

    public override void DeactivateUpgrade()
    {
        Debug.Log("Invisible player deactivated");
    }
}
