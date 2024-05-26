using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action EnemyHitted;
    public event Action<EnemyBodyPart, Vector3> EnemyDie;

    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private EnemyHealthBar _healthBarPrefab;

    private float _health;
    private EnemyHealthBar _healthBar;

    private void Awake() => _health = _maxHealth;

    private void Start()
    {
        _healthBar = Instantiate(_healthBarPrefab, transform);
        _healthBar.Initialize(transform);
        _healthBar.SetHealth(_health, _maxHealth);
    }

    public void ApplyDamage(float value, EnemyBodyPart hittedPart, Vector3 direction)
    {
        _health -= value;
        _healthBar.SetHealth(_health, _maxHealth);

        if (_health <= 0)
        {
            _healthBar.Deactivate();

            EnemyDie?.Invoke(hittedPart, direction);
        }
        else
        {
            EnemyHitted?.Invoke();
        }
    }
}