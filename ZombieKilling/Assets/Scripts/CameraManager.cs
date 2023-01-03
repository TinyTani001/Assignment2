using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform _cameraAnchor;
    [SerializeField] private float _followAcceleration;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private bool _isCharacterCamera;

    private void FixedUpdate()
    {
        if (_playerData.Player == null || !_isCharacterCamera) return;
        _cameraAnchor.position = Vector3.Lerp(_cameraAnchor.position, _playerData.Player.transform.position, _followAcceleration * Time.deltaTime);
    }

    private void Update()
    {
        if (_playerData.Player == null || _isCharacterCamera) return;
        _cameraAnchor.position = Vector3.Lerp(_cameraAnchor.position, _playerData.Player.transform.position, _followAcceleration * Time.deltaTime);
    }
}
