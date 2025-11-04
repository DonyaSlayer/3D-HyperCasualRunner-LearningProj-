using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float _reloadingTime;
    public GameObject bulletPrefab;
    [SerializeField] private Transform _firingPoint;

    public Animator animator;

    private Coroutine _shootingTimer;

    public void StartShooting()
    {
        if (_shootingTimer != null) return;
        _shootingTimer = StartCoroutine(ShootingTimer());
        animator.SetBool("Shooting", true);
    }
    public void StopShooting()
    {
        if (_shootingTimer == null) return;

        StopCoroutine(_shootingTimer);
        _shootingTimer = null;
        animator.SetBool("Shooting", false);
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
