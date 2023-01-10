using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;
    [SerializeField] private Slider _upgradeBarImage;

    private void Start()
    {
        _upgradeCollectibeData.OnZombieKillPercentUpdated += percent =>
        {
            _upgradeBarImage.value = percent;
            if (percent >= 0.99f)
            {
                _upgradeBarImage.value = 0f;
            }
        };
    }
}
