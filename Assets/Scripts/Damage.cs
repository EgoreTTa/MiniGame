using UnityEngine;

public class Damage
{
    private BaseMob _owner;
    private GameObject _healthBy;
    private float _countHealth;
    private TypesDamage _typeDamage;

    public BaseMob Owner => _owner;
    public GameObject HealthBy => _healthBy;
    public float CountHealth => _countHealth;
    public TypesDamage TypeDamage => _typeDamage;

    public Damage(
        BaseMob owner,
        GameObject healthBy,
        float countHealth,
        TypesDamage typeDamage)
    {
        _owner = owner;
        _healthBy = healthBy;
        _countHealth = countHealth;
        _typeDamage = typeDamage;
    }
}