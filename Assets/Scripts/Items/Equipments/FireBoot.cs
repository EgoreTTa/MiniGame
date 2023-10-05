namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Effects;

    [DisallowMultipleComponent]
    public class FireBoot : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] [Range(.02f, 100)] private float _intervalRegeneration;
        [SerializeField] private GameObject _fireCirclePrefab;
        private EquipmentSets _equipmentSet = EquipmentSets.FireBoot;

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
            var effect = HelpLibraryForEquipmentSet.CheckSkill(_owner) as FireCircle;
            if (effect != null)
            {
                Destroy(effect);
                effect.IsActive = false;
            }
        }
    }
}