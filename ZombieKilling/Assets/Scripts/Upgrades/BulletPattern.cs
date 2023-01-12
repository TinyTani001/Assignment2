using UnityEngine;

[CreateAssetMenu(fileName = "BulletPattern.asset", menuName = "Scriptable Objects/Upgrades/Bullet Pattern")]
public class BulletPattern : UpgradeBase
{
    [SerializeField] private float _decreaseBulletSpawnArcByAngle;
    [SerializeField] private PlayerDataSO _playerData;
    public override void ActivateUpgrade()
    {
        _playerData.SetBulletSpawnPattern((bulletObject, bulletSpawnPosition, playerAimRotation) =>
        {
            float angle = 270f + _decreaseBulletSpawnArcByAngle / 2f;
            int numberOfBullets = 5;
            float increaseAngleby = (180f - _decreaseBulletSpawnArcByAngle) / (numberOfBullets - 1);
            for (int i = 0; i < 5; i++)
            {
                Quaternion bulletRotation = playerAimRotation * Quaternion.AngleAxis(angle, Vector3.up);
                Instantiate(bulletObject, bulletSpawnPosition, bulletRotation);
                angle = (angle + increaseAngleby) % 360f;
            }
        });
    }

    public override void DeactivateUpgrade()
    {
        _playerData.ResetUpgradables();
    }
}
