using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),
    typeof(Collider2D))]
public class Trap : MonoBehaviour
{
    public enum StatesOfTrap
    {
        Standby,
        Charging,
        Attack
    }

    [SerializeField] private StatesOfTrap _stateOfTrap;
    [SerializeField] private float _timeCharging;
    [SerializeField] private float _timeAttack;
    [SerializeField] private float _damageCount;
    private Damage _damage;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _spriteStandby;
    [SerializeField] private Sprite _spriteCharging;
    [SerializeField] private Sprite _spriteAttack;
    [SerializeField] private float _intervalDamaged;
    private List<IHealthSystem> _healthSystems = new();

    public StatesOfTrap StateOfTrap => _stateOfTrap;

    private void Awake()
    {
        _damage = new Damage(null, gameObject, _damageCount, TypesDamage.Clear);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void IntoStandby()
    {
        _stateOfTrap = StatesOfTrap.Standby;
        _spriteRenderer.sprite = _spriteStandby;
        CancelInvoke(nameof(Damaged));
    }

    private void IntoCharging()
    {
        _stateOfTrap = StatesOfTrap.Charging;
        _spriteRenderer.sprite = _spriteCharging;
        Invoke(nameof(IntoAttack), _timeCharging);
    }

    private void IntoAttack()
    {
        _stateOfTrap = StatesOfTrap.Attack;
        _spriteRenderer.sprite = _spriteAttack;
        Invoke(nameof(IntoStandby), _timeAttack);
        InvokeRepeating(nameof(Damaged), 0f, _intervalDamaged);
    }

    private void Damaged()
    {
        foreach (var healthSystem in _healthSystems) healthSystem.TikeDamage(_damage);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.isTrigger is false)
        {
            if (_stateOfTrap == StatesOfTrap.Standby)
                if (collider.GetComponent<Player>() is { } player)
                    IntoCharging();
            if (collider.gameObject.GetComponent<IHealthSystem>() is { } healthSystem)
                _healthSystems.Add(healthSystem);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.isTrigger is false)
            if (collider.GetComponent<IHealthSystem>() is { } healthSystem)
                _healthSystems.Remove(healthSystem);
    }
}