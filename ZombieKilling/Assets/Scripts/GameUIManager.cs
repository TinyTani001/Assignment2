using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameUIDataSO _gameUIData;
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;
    [SerializeField] private Slider _upgradeBarImage;
    [SerializeField] private TMP_Text _playerHealthText, _bulletDamageText, _fireRateText;

    private float _lastTick;
    private int _secondsPassed, _activeUpgradeIndex;

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

        _gameUIData.OnFireRateUpdated += fireRate => _fireRateText.text = fireRate.ToString();
        _gameUIData.OnDamageUpdated += damage => _bulletDamageText.text = damage.ToString();
        _gameUIData.OnHealthUpdated += health => _playerHealthText.text = health.ToString();

        _lastTick = Time.time + 1f;
    }

    private void Update()
    {
        if(Time.time > _lastTick)
        {
            _lastTick = Time.time + 1f;
            _secondsPassed++;
            if(_secondsPassed == 2)
            {
                _secondsPassed = 0;
                _activeUpgradeIndex = (_activeUpgradeIndex + 1) % 5;
                _upgradeCollectibeData.ActivateUpgradeOnIndex(_activeUpgradeIndex);
            }
        }
    }

    private void OnDestroy()
    {
        _gameUIData.ClearResources();
    }
}
