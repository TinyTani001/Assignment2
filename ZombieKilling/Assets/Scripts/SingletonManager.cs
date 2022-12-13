using UnityEngine;

[DefaultExecutionOrder(-200)]
public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance { get; private set; }

    public Player Player { get; set; }

    private void Awake()
    {
        Instance = this;
    }
}
