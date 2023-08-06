using UnityEngine;

[DisallowMultipleComponent]
public class Boyara : BaseItem
{
    private int _damageBoost;
    [SerializeField] private int _minRange;
    [SerializeField] private int _maxRange;

    private void Start()
    {
        _damageBoost = Random.Range(_minRange, _maxRange);
    }

    public override void Use(BaseMob target)
    {
        target.Damage += _damageBoost;
        Destroy(gameObject);
    }
}