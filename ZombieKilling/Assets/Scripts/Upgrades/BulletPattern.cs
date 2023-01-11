using UnityEngine;

[CreateAssetMenu(fileName = "BulletPattern.asset", menuName = "Scriptable Objects/Upgrades/Bullet Pattern")]
public class BulletPattern : UpgradeBase
{
    public override void ActivateUpgrade()
    {
        Debug.Log("Bullet pattern activated");
    }

    public override void DeactivateUpgrade()
    {
        Debug.Log("Bullet Pattern deactivated");
    }
}
