namespace Items.Equipments
{
    using Effects;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class FireBoot : BaseItem, IEquipment
    {
        private BaseHealthSystem _healthSystem;
        private Health _health;
        private readonly EquipmentSets _equipmentSet = EquipmentSets.FireBoot;
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] [Min(.02f)] private float _intervalRegeneration;
        [SerializeField] private GameObject _fireCirclePrefab;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => _equipmentSet;

        private void Regeneration()
        {
            _healthSystem.TakeHealth(_health);
        }

        public void Equip()
        {
            _healthSystem = _owner.GetComponent<BaseHealthSystem>();
            _healthSystem.MaxHealth += _changeMaxHealth;
            _health = new Health(_owner, gameObject, _changeRegeneration);
            InvokeRepeating(nameof(Regeneration), 0f, _intervalRegeneration);
            HelpLibraryForEquipmentSet.CheckSet(_owner, _fireCirclePrefab, EquipmentSets.FireSet);
        }

        public void Unequip()
        {
            _healthSystem.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regeneration));
            var effect = HelpLibraryForEquipmentSet.CheckSkill(_owner);
            if (effect is FireCircleEffect fireCircleEffect)
            {
                Destroy(fireCircleEffect);
                fireCircleEffect.IsActive = false;
            }
        }
    }
}