using UnityEngine;

public class BulletBox : MonoBehaviour
{
    [SerializeField] private float _timer;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _shootSoundClip;
    public string bulletType;
    private void OnTriggerEnter(Collider other)
    {
        TeamController.instance.SetBullets (_bulletPrefab, _timer, bulletType, _shootSoundClip);
        Destroy(gameObject);
    }
}
