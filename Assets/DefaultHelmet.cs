using UnityEngine;

[DisallowMultipleComponent]
public class DefaultHelmet : BaseItem, IEquipment
{
    [SerializeField] private float _changeMaxHealth;
    [SerializeField] private TypesEquipment _typeEquipment;

    public TypesEquipment TypeEquipment => _typeEquipment;

    public void Equip()
    {
        _owner.MaxHealth += _changeMaxHealth;
    }

    public void Unequip()
    {
        _owner.MaxHealth -= _changeMaxHealth;
    }
}