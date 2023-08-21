using UnityEngine;

public class Damage
{
    private BaseMob _owner;
    private GameObject _damageBy;
    private float _countDamage;
    private TypesDamage _typeDamage;

    public BaseMob Owner => _owner;
    public GameObject DamageBy => _damageBy;
    public float CountDamage => _countDamage;
    public TypesDamage TypeDamage => _typeDamage;

    public Damage(
        BaseMob owner,
        GameObject damageBy,
        float countDamage,
        TypesDamage typeDamage)
    {
        _owner = owner;
        _damageBy = damageBy;
        _countDamage = countDamage;
        _typeDamage = typeDamage;
    }
}