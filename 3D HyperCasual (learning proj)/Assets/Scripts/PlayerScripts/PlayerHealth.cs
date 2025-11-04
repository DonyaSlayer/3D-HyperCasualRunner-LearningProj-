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

    [Header("UI")]

    [SerializeField] private Image _healthFillImage;


    private void FixedUpdate()
    {
        if (_canRegenerate)
        {
            _health += _healthRegeneration;
        }
        
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthFillImage.fillAmount = _health / _maxHealth;

    }

    public void Damage(float damage)
    {
        if (_canTakeDamage == true)
        {
            _health -= damage;
            _canTakeDamage = false;
            _canRegenerate = false;
            StartCoroutine(DamageCooldown());
        }

        if(_health == 0 )
        {
            SceneManager.LoadScene(0);
        }
    }
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds( _damageCooldown);
        _canTakeDamage = true;
        _canRegenerate = true;
    }
}
