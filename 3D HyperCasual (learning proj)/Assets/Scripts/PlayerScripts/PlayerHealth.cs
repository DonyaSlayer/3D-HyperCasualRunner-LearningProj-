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
        _isDead = true;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds( _damageCooldown);
        _canTakeDamage = true;
        _canRegenerate = true;
    }
}
