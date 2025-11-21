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
        transform.Translate(Vector3.forward * _flySpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Soldier"))
        {
            if (TeamController.instance.soldiers.Count > 0)
            {
                TeamController.instance.RemoveSoldier();
            }
            else
            {
                other.gameObject.GetComponent<PlayerHealth>().Damage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
