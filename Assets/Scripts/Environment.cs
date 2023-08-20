using System;
using UnityEngine;

public class Environment : MonoBehaviour, IHealthSystem
{
    [SerializeField] protected float _health;
    [SerializeField] protected float _minHealth;
    [SerializeField] protected float _maxHealth;

    public float Health
    {
        get => _health;
        private set
        {
            if (value < _minHealth)
            {
                value = _minHealth;
                Destruction();
            }

            if (value > _maxHealth)
            {
                value = _maxHealth;
            }

            _health = value;
        }
    }

    private void Destruction()
    {
        GetComponent<SpriteRenderer>().color = Color.white.linear * .2f;
        ItemDropSystem.Drop(transform.position);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
        Destroy(this);
    }

    public float MinHealth
    {
        get => _minHealth;
        set
        {
            if (value < 0) value = 0;
            if (value > _maxHealth) value = _maxHealth;
            _minHealth = value;
        }
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (value < _maxHealth) value = _maxHealth;
            _maxHealth = value;
        }
    }

    public void ChangeHealth(Health health)
    {
        Health -= health.TypeDamage switch
        {
            TypesDamage.Physical => health.CountHealth / 2,
            TypesDamage.Magical => health.CountHealth * 2,
            TypesDamage.Clear => health.CountHealth,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}