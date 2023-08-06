using UnityEngine;

[DisallowMultipleComponent]
public class HangoverEffect : BaseEffect
{
    [SerializeField] private float _delay;
    [SerializeField] private float _intervalTakeDamage;
    private float _timerIntervalTakeDamage;
    [SerializeField] private bool _takingDamage;
    [SerializeField] private float _damage;
    private BaseMob _target;

    public override void StartEffect(BaseMob target)
    {
        _takingDamage = true;
    }

    public void PostponeEffect()
    {
        _takingDamage = false;
        Invoke(nameof(StartEffect), _delay);
    }

    private void TakeDamage()
    {
        var damage = new Damage(null, null, TypeDamage.Clear, _damage);
        _target.TakeDamage(damage);
    }

    private void Awake()
    {
        _target = GetComponent<BaseMob>();
        if (_target == null) 
            Destroy(this);
        else 
            Invoke(nameof(StartEffect), _delay);
    }

    private void Update()
    {
        if (_takingDamage is true)
        {
            if (_timerIntervalTakeDamage < _intervalTakeDamage)
            {
                _timerIntervalTakeDamage += Time.deltaTime;
            }
            else
            {
                _timerIntervalTakeDamage -= _intervalTakeDamage;
                TakeDamage();
            }
        }
    }
}