using UnityEngine;

public class UpgradeCollectible : MonoBehaviour
{
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;

    private int _upgradeIndex = -1;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _upgradeCollectibeData.UpgradeCollected(_upgradeIndex);
            Destroy(gameObject);
        }
    }

    public void SetUpgradeIndex(int index) => _upgradeIndex = index;
}
