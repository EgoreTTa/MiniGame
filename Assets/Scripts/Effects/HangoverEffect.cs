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
        var health = new Health(null, null, _damage);
        (_target as IHealthSystem).ChangeHealth(health);
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