namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Assets.Scripts.Skills;

    [DisallowMultipleComponent]
    public class FireArmor : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] private float _intervalRegeneration;
        [SerializeField] private BaseItem ElememtSetHelmet;
        [SerializeField] private BaseItem ElememtSetBoot;
        [SerializeField] private GameObject FireCirlcePrefab;
        private GameObject FireCirlceSpell;
        public TypesEquipment TypeEquipment => _typeEquipment;

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
            FireCirlceSpell = CheckSkill();
            if (FireCirlceSpell is not null)
            {
                Destroy(FireCirlceSpell);
                FireCirlceSpell.GetComponent<FireCirle>().Active = false;
            }
        }

        public void CheckSet()
        {
            if (_owner.Inventory.Helmet?.NameItem == ElememtSetHelmet.NameItem &&
                _owner.Inventory.Boot?.NameItem == ElememtSetBoot.NameItem)
            {
                FireCirlceSpell = CheckSkill();
                if (FireCirlceSpell == null)
                {
                    FireCirlceSpell = Instantiate(FireCirlcePrefab, _owner.gameObject.transform);
                    Debug.Log("ОО жара пошла");
                }
                else
                {
                    Debug.Log("А скилл то уже есть");
                }
            }
            else
                Debug.Log("Сэт не одет");
        }

        public GameObject CheckSkill()
        {
            if (_owner.GetComponentsInChildren<FireCirle>() is { } fireCirles)
            {
                foreach (var fireCirle in fireCirles)
                {
                    if (fireCirle.GetComponent<FireCirle>().Active == true)
                    {
                        return fireCirle.gameObject;
                    }
                }
            }

            return null;
        }
    }
}