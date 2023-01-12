using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameUIDataSO _gameUIData;
    [SerializeField] private UpgradeCollectibleDataSO _upgradeCollectibeData;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private Slider _upgradeBarImage;
    [SerializeField] private Transform _starRingTransform;
    [SerializeField] private TMP_Text _playerHealthText, _bulletDamageText, _fireRateText;
    [SerializeField] private GameObject _activeAbilityCover, _resultScreen;
    [SerializeField] private Sprite _winSprite, _looseSprite;
    [SerializeField] private Image _activeAbilityIcon, _resultScreenMessageImage;
    [SerializeField] private AbilitiesUIElementData[] _abilitiesUIElements;
    [SerializeField] private GameObject[] _uiToDisableOnPlayerDeath;

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

        _playerData.OnPlayerDead += () =>
        {
            _resultScreenMessageImage.sprite = _looseSprite;
            _lastTick = 0f;
            foreach (GameObject uiItem in _uiToDisableOnPlayerDeath)
            {
                uiItem.SetActive(false);
            }
            StartCoroutine(ShowResultScreen());
        };

        _upgradeCollectibeData.OnUpgradeCollected += (index, upgrade) =>
        {
            _abilitiesUIElements[index].Cover.SetActive(false);
        };

        _gameUIData.OnFireRateUpdated += fireRate => _fireRateText.text = fireRate.ToString();
        _gameUIData.OnDamageUpdated += damage => _bulletDamageText.text = damage.ToString();
        _gameUIData.OnHealthUpdated += health => _playerHealthText.text = health.ToString();

        _lastTick = Time.time + 1f;
    }

    private void Update()
    {
        if(_lastTick > 0f && Time.time > _lastTick)
        {
            _lastTick = Time.time + 1f;
            _secondsPassed++;
            if(_secondsPassed == 2)
            {
                _secondsPassed = 0;
                _activeUpgradeIndex = _activeUpgradeIndex - 1 < 0 ? 4 : _activeUpgradeIndex - 1;
                bool abilityActivated = _upgradeCollectibeData.ActivateUpgradeOnIndex(_activeUpgradeIndex);
                _activeAbilityCover.SetActive(!abilityActivated);
                if (abilityActivated) _activeAbilityIcon.sprite = _abilitiesUIElements[_activeUpgradeIndex].Icon.sprite;
            }
            _starRingTransform.rotation *= Quaternion.AngleAxis(-36f, Vector3.forward);
            foreach (AbilitiesUIElementData elem in _abilitiesUIElements)
            {
                elem.CorrectRotation();
            }
        }
    }

    private void OnDestroy()
    {
        _gameUIData.ClearResources();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator ShowResultScreen()
    {
        yield return new WaitForSeconds(_gameUIData.ResultScreenDelay);
        _resultScreen.SetActive(true);
    }

    [Serializable]
    private struct AbilitiesUIElementData
    {
        public Transform ParentTransform;
        public GameObject Cover;
        public Image Icon;

        public void CorrectRotation()
        {
            ParentTransform.rotation = Quaternion.identity;
        }
    }
}
