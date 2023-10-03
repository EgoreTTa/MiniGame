namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Abilities;

    [DisallowMultipleComponent]
    public class FireBoot : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] private float _intervalRegeneration;
        [SerializeField] private BaseItem _elementSetHelmet;
        [SerializeField] private BaseItem _elementSetArmor;
        [SerializeField] private GameObject FireCirlcePrefab;
        private GameObject FindSkill;
        [SerializeField] private EquipmentSets _equipmentSet;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => _equipmentSet;

        private void Regen()
        {
            var health = new Health(_owner, gameObject, _changeRegeneration);
            (_owner as IHealthSystem).TakeHealth(health);
        }

        public void Equip()
        {
            CheckSet();
            _owner.MaxHealth += _changeMaxHealth;
            InvokeRepeating(nameof(Regen), 0f, _intervalRegeneration);
        }

        public void Unequip()
        {
            _owner.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regen));
            FindSkill = CheckSkill();
            if (FindSkill != null)
            {
                Destroy(FindSkill);
                FindSkill.IsActive = false;
            }
        }

        public void CheckSet()
        {
            if (_owner.Inventory.Armor?.NameItem == _elementSetArmor.NameItem
                && _owner.Inventory.Helmet?.NameItem == _elementSetHelmet.NameItem)
            {
                FindSkill = CheckSkill();
                if (FindSkill == null)
                {
                    FindSkill = Instantiate(FireCirlcePrefab, _owner.gameObject.transform);
                    FindSkill.name = "FireCirlceFireSet";
                }
            }
        }

        public GameObject CheckSkill()
        {
            if (_owner.GetComponentsInChildren<FireCircle>() is { } fireCirles)
            {
                foreach (var fireCirle in fireCirles)
                {
                    if (fireCirle.GetComponent<FireCircle>().Active == true)
                    {
                        return fireCirle.gameObject;
                    }
                }
            }

            return null;
        }
    }
}