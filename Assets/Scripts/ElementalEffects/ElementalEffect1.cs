using UnityEngine;

[DisallowMultipleComponent]
public class ElementalEffect1 : BaseElementalEffect
{
    [SerializeField] private float _damageCount = 2f;
    [SerializeField] private float _timeOfAction = 10f;
    private float _timerOfAction;
    [SerializeField] private float _timeDamage = .2f;
    private float _timerDamage;

    private void Start()
    {
        _typeElement = TypesElement.ElementalEffect1;
        CombineEffect();
    }

    private void Update()
    {
        if (_timerOfAction < _timeOfAction)
        {
            _timerOfAction += Time.deltaTime;
            if (_timerDamage < _timeDamage)
            {
                _timerDamage += Time.deltaTime;
            }
            else
            {
                _timerDamage -= _timeDamage;
                MakeDamage();
            }
        }
        else
        {
            Destroy(this);
        }
    }

    private void MakeDamage()
    {
        var health = new Health(
            _owner,
            null,
            _damageCount);
        (_target as IHealthSystem).ChangeHealth(health);
    }
}