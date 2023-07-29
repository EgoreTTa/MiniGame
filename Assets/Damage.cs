using UnityEngine;

public class Damage
{
    private BaseMob _owner;
    private GameObject _parent;
    private TypeDamage _typeDamage;
    private float _countDamage;

    public BaseMob Owner => _owner;
    public GameObject Parent => _parent;
    public TypeDamage TypeDamage => _typeDamage;
    public float CountDamage => _countDamage;

    public Damage(
        BaseMob owner,
        GameObject parent,
        TypeDamage typeDamage,
        float countDamage)
    {
        _owner = owner;
        _parent = parent;
        _typeDamage = typeDamage;
        _countDamage = countDamage;
    }
}