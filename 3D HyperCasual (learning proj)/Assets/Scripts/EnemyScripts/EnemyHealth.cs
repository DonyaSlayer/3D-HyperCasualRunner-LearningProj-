using System.Collections;
using System.Drawing;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isDead;
    [SerializeField] private float _deathDelay = 0.2f;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _damageColor;
    [SerializeField] private float flashDuration = 0.1f;

    private Material _originalMaterial;

    private void Start()
    {
        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        _originalMaterial = _renderer.material;
    }

    private bool _isDying = false;

    private void Update()
    {
        _animator.SetBool("IsDead", _isDead);
    }
    public void Damage(float damage)
    {
        if (_isDying) return;
        _health -= damage;
        StartCoroutine(FlashDamage());
        if (_health <= 0)
        {
            _isDead = true;
            StartCoroutine(DeathSequence());
        }
    }
    private IEnumerator FlashDamage()
    {
        _renderer.material = _damageColor;
        yield return new WaitForSeconds(flashDuration);
        _renderer.material = _originalMaterial;
    }

    private IEnumerator DeathSequence()
    {
        _animator.Play("ZombieDeath");
        if (TryGetComponent<EnemyMovement>(out EnemyMovement move))
            move.StopMoving();
        if (TryGetComponent<Collider>(out Collider col))
            col.enabled = false;
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;
        yield return new WaitForSeconds(_deathDelay);
        if (_coinPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            if (transform.parent != null)
                spawnPos = transform.parent.TransformPoint(transform.localPosition);
            Instantiate(_coinPrefab, spawnPos + Vector3.up * 0.5f, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
