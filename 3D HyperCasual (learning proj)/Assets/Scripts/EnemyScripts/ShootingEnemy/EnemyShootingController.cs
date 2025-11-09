using UnityEngine;
using System.Collections;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private float _reloadingTime;
    [SerializeField] private float _initialDelay = 1.0f;
    public GameObject bulletPrefab;
    [SerializeField] private Transform _firingPoint;
    private Coroutine _shootingTimer;
    private Coroutine _delayTimer;
    [SerializeField] private Animator _animator;

    public void StartShooting()
    {
        if (_shootingTimer != null || _delayTimer != null) return;
        _delayTimer = StartCoroutine(StartShootingWithDelay());
    }
    public void StopShooting()
    {
        if (_delayTimer != null)
        {
            StopCoroutine(_delayTimer);
            _delayTimer = null;
        }
        if (_shootingTimer == null) return;
        StopCoroutine(_shootingTimer);
        _shootingTimer = null;
        _animator.SetBool("IsShooting", false);
    }
    private IEnumerator StartShootingWithDelay()
    {
        _animator.SetBool("IsShooting", true);
        yield return new WaitForSeconds(_initialDelay);
        _shootingTimer = StartCoroutine(ShootingTimer());
        _delayTimer = null; 
    }
    private IEnumerator ShootingTimer()
    {
        while (true)
        {
            Instantiate(bulletPrefab, _firingPoint.position, _firingPoint.rotation);
            yield return new WaitForSeconds(_reloadingTime);
        }
    }
}
