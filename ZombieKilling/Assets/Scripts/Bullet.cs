using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bulletLifeTime, _bulletSpeed;
    [SerializeField] private PlayerDataSO _playerData;

    private int _damage;

    private void Start()
    {
        _bulletLifeTime = Time.time + _bulletLifeTime;
        _damage = _playerData.BulletDamage;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * _bulletSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Time.time > _bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            if (coll.gameObject.TryGetComponent(out Zombie zombie))
            {
                if(!zombie.HitZombie(_damage)) Destroy(gameObject);
            }
        }
    }
}
