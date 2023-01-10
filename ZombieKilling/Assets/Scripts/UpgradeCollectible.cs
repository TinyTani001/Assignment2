using UnityEngine;

public class UpgradeCollectible : MonoBehaviour
{
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _upgradeCollectibeData.UpgradeCollected();
            Destroy(gameObject);
        }
    }
}
