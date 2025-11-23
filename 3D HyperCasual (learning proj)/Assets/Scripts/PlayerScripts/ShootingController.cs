using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float _reloadingTime;
    public GameObject bulletPrefab;
    [SerializeField] private Transform _firingPoint;

    public Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource _shootingAudioSource;
    [SerializeField] private AudioClip _shootClip;
    private AudioClip _currentShootClip;

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
    public void UpdateBulletSettings(AudioClip newShootClip)
    {
        _currentShootClip = newShootClip;
    }

    private IEnumerator ShootingTimer()
    {
        while (true)
        { 
            Instantiate(bulletPrefab, _firingPoint.position, Quaternion.identity, null);
            AudioClip clipToPlay = (_currentShootClip != null)
                                   ? _currentShootClip
                                   : _shootClip;
            if (_shootingAudioSource != null && clipToPlay != null)
            {
                _shootingAudioSource.PlayOneShot(clipToPlay, 1f);
            }   
            yield return new WaitForSeconds(_reloadingTime);
        }
    }
}
