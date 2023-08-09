using UnityEngine;

[DisallowMultipleComponent]
public class DefaultBoot : BaseItem, IEquipment
{
    [SerializeField] private float _changeMaxHealth;
    [SerializeField] private TypesEquipment _typeEquipment;

    public TypesEquipment TypeEquipment => _typeEquipment;

    public void Equip() { _parent.MaxHealth += _changeMaxHealth; }

    public void Unequip() { _parent.MaxHealth -= _changeMaxHealth; }
}