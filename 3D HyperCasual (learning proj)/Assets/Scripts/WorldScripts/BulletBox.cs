using UnityEngine;

public class BulletBox : MonoBehaviour
{
    [SerializeField] private float _timer;
    [SerializeField] private GameObject _bulletPrefab;
    private void OnTriggerEnter(Collider other)
    {
        TeamController.instance.SetBullets (_bulletPrefab, _timer);
        Destroy(gameObject);
    }
}
