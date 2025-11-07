using UnityEngine;
using System.Collections;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private float _reloadingTime;
    public GameObject bulletPrefab;
    [SerializeField] private Transform _firingPoint;

    private Coroutine _shootingTimer;

    public void StartShooting()
    {
        if (_shootingTimer != null) return;
        _shootingTimer = StartCoroutine(ShootingTimer());
    }
    public void StopShooting()
    {
        if (_shootingTimer == null) return;

        StopCoroutine(_shootingTimer);
        _shootingTimer = null;
    }

    private IEnumerator ShootingTimer()
    {
        while (true)
        {
            Instantiate(bulletPrefab, _firingPoint.position, Quaternion.identity, null);
            yield return new WaitForSeconds(_reloadingTime);
        }
    }
}
