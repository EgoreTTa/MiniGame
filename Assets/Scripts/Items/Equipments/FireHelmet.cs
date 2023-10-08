namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Effects;

    [DisallowMultipleComponent]
    public class FireHelmet : BaseItem, IEquipment
    {
        private IHealthSystem _healthSystem;
        private Health _health;
		private EquipmentSets _equipmentSet = EquipmentSets.FireHelmet;
		[SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] [Range(.02f, 100)] private float _intervalRegeneration;
        [SerializeField] private GameObject _fireCirclePrefab;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => _equipmentSet;

        private void Regeneration()
        {
            _healthSystem.TakeHealth(_health);
        }

        public void Equip()
        {
            _healthSystem = _owner.GetComponent<IHealthSystem>();
            _healthSystem.MaxHealth += _changeMaxHealth;
            _health = new Health(_owner, gameObject, _changeRegeneration);
            InvokeRepeating(nameof(Regeneration), 0f, _intervalRegeneration);
            HelpLibraryForEquipmentSet.CheckSet(_owner, _fireCirclePrefab, EquipmentSets.FireSet);
        }

        public void Unequip()
        {
            _healthSystem.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regeneration));
            var effect = HelpLibraryForEquipmentSet.CheckSkill(_owner) as FireCircle;
            if (effect != null)
            {
                Destroy(effect);
                effect.IsActive = false;
            }
        }
    }
}