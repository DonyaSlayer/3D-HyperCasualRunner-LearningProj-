using UnityEngine;

public class BulletBox : MonoBehaviour
{
    [SerializeField] private float _timer;
    [SerializeField] private GameObject _bulletPrefab;
    public string bulletType;
    private void OnTriggerEnter(Collider other)
    {
        TeamController.instance.SetBullets (_bulletPrefab, _timer, bulletType);
        Destroy(gameObject);
    }
}
