using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _flyTime;
    [SerializeField] private float _damage;

    private void Start()
    {
        Destroy(gameObject, _flyTime);
    }
    private void FixedUpdate()
    {
        transform.position += transform.forward * _flySpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.Damage(_damage);
            Destroy(gameObject);
        }
    }
}
