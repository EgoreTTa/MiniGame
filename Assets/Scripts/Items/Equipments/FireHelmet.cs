namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using System.Collections;
    using Enemies;
    using NoMonoBehaviour;

    [DisallowMultipleComponent]

    public class FireHelmet : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private GameObject FireCircke;
        [SerializeField] private TypesEquipment _typeEquipment;
        public TypesEquipment TypeEquipment => _typeEquipment;
        private void Regen()
        {
            var health = new Health(_owner, gameObject, _changeRegeneration);
            (_owner as IHealthSystem).TakeHealth(health);
        }
        public void Equip()
        {
            _owner.MaxHealth += _changeMaxHealth;
            InvokeRepeating("Regen", 0f, 1f);
        }
        public void Unequip()
        {
            _owner.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regen));
        } 
    }
}

