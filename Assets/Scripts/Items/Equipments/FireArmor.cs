namespace Assets.Scripts.Items.Equipments
{
    using System;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Abilities;

    [DisallowMultipleComponent]
    public class FireArmor : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] [Range(1, 10)] private float _intervalRegeneration;
        [SerializeField] private BaseItem _elementSetHelmet;
        [SerializeField] private BaseItem _elementSetBoot;
        [SerializeField] private GameObject _fireCirclePrefab;
        private EquipmentSets _equipmentSet = EquipmentSets.FireArmor;
        private FireCircle FindSkill;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => _equipmentSet;

        private void Regen()
        {
            var health = new Health(_owner, gameObject, _changeRegeneration);
            (_owner as IHealthSystem).TakeHealth(health);
        }

        public void Equip()
        {
            HelpLibraryForEquipmentSet.CheckSet(_owner, _fireCirclePrefab, EquipmentSets.FireSet);
            _owner.MaxHealth += _changeMaxHealth;
            InvokeRepeating(nameof(Regen), 0f, _intervalRegeneration);
        }

        public void Unequip()
        {
            _owner.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regen));
            FindSkill = HelpLibraryForEquipmentSet.CheckSkill(_owner) as FireCircle;
            if (FindSkill != null)
            {
                Destroy(FindSkill);
                FindSkill.IsActive = false;
            }
        }
    }
}