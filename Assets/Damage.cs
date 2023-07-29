using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    private BaseMob _owner;
    private GameObject _parent;
    private TypeDamage _typeDamage;
    private List<BaseEffect> _effects;

    public Damage (
        BaseMob owner, 
        GameObject parent, 
        TypeDamage typeDamage,
        BaseEffect[] effects)
    {
        _owner = owner;
        _parent = parent;
        _typeDamage = typeDamage;
        _effects = effects;
    }

}

public enum TypeDamage
{
    Physical,
    Magical,
    Clear
}