using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Values")]

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private float _healthRegeneration;
    [SerializeField] private float _damageCooldown;
    private bool _canRegenerate = false;
    private bool _canTakeDamage = true;
    private bool _isDead = false;

    [Header("UI")]
    [SerializeField] private Image _healthFillImage;


    [Header("Other")]
    [SerializeField] private Animator _animator;

    [Header("Damaged")]
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Material _damageColor;
    [SerializeField] private float flashDuration = 0.1f;
    private Material[] _originalMaterials;

    private void Start()
    {
        if (_renderers == null || _renderers.Length == 0)
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }
        _originalMaterials = new Material[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalMaterials[i] = _renderers[i].material;
        }
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        if (_canRegenerate)
        {
            _health += _healthRegeneration;
        }
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthFillImage.fillAmount = _health / _maxHealth;

        if (_health <= 0 && !_isDead)
        {
            Die();
        }
    }

    public void Damage(float damage)
    {
        if (_isDead) return;
        StartCoroutine(FlashDamage());
        if (_canTakeDamage == true)
        {
            _health -= damage;
            _canTakeDamage = false;
            _canRegenerate = false;
            StartCoroutine(DamageCooldown());
        }

        if (_health <= 0 && !_isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        _animator.SetBool("IsDead", true);
        _isDead = true;
        if (TryGetComponent<PlayerMovement>(out PlayerMovement move))

            move.StopMoving();

        if (TryGetComponent<Collider>(out Collider col))

            col.enabled = false;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))

            rb.isKinematic = true;
        StartCoroutine(ShowGameOverWithDelay(3.0f));
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds( _damageCooldown);
        _canTakeDamage = true;
        _canRegenerate = true;
    }
    private IEnumerator ShowGameOverWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverPanelSmoothly();
        }
        gameObject.SetActive(false);
    }
    private IEnumerator FlashDamage()
    {
        foreach (Renderer r in _renderers)
        {
            r.material = _damageColor;
        }
        yield return new WaitForSeconds(flashDuration);
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = _originalMaterials[i];
        }
    }
}
