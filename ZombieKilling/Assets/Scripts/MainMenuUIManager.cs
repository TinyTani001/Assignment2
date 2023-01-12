using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{

    private float _sceneLoadDelay = 0.05f, _currentLoadTime = -1f;

    private void Update()
    {
        if (_currentLoadTime > 0f && Time.time > _currentLoadTime + _sceneLoadDelay)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LoadGameScene()
    {
        _currentLoadTime = Time.time;
    }
}
