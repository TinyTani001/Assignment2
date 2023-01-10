using UnityEngine;

[CreateAssetMenu(fileName = "UIData.asset", menuName = "Scriptable Objects/UI Data")]
public class UIDataSO : ScriptableObject
{
    [SerializeField] private int _zombieKillCountToUpgrade;


}
