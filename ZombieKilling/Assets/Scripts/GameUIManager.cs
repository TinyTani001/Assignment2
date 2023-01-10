using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private ZombieSpawnDataSO _zombieSpawnData;
    [SerializeField] private int _zombieKillsToUpgrade;
    [SerializeField] private Slider _upgradeBarImage;

    private int _currentZombiKillsToUpgrade;

    private void Start()
    {
        _zombieSpawnData.OnZombieSpawnRequest += () =>
        {
            _currentZombiKillsToUpgrade++;
            _upgradeBarImage.value = Mathf.InverseLerp(0f, _zombieKillsToUpgrade, _currentZombiKillsToUpgrade);
            if (_currentZombiKillsToUpgrade == _zombieKillsToUpgrade)
            {
                _currentZombiKillsToUpgrade = 0;
                _upgradeBarImage.value = 0f;
            }
        };
    }
}
