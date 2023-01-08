using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bulletLifeTime;

    private void Start()
    {
        _bulletLifeTime = Time.time + _bulletLifeTime;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * 16f * Time.fixedDeltaTime);
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
        if (coll.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if (coll.gameObject.TryGetComponent(out Zombie zombie))
            {
                if(!zombie.HitZombie(20)) Destroy(gameObject);
            }
        }
    }
}
