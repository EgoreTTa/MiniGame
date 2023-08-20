using UnityEngine;

public class Damage
{
    private BaseMob _owner;
    private GameObject _damagedBy;
    private TypesDamage _typeDamage;
    private float _countDamage;

    public BaseMob Owner => _owner;
    public GameObject DamagedBy => _damagedBy;
    public TypesDamage TypeDamage => _typeDamage;
    public float CountDamage => _countDamage;

    public Damage(
        BaseMob owner,
        GameObject damagedBy,
        TypesDamage typeDamage,
        float countDamage)
    {
        _owner = owner;
        _damagedBy = damagedBy;
        _typeDamage = typeDamage;
        _countDamage = countDamage;
    }
}
