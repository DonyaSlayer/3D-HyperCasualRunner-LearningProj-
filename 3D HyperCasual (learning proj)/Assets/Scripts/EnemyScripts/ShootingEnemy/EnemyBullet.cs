using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
        transform.position += transform.position * _flySpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.Damage(_damage);
            Destroy(gameObject);
        }
    }
}
