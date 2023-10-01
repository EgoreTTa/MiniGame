namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;
    using Skills;
    using UnityEditor.Presets;

    [DisallowMultipleComponent]
    public class FireHelmet : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] private float _intervalRegeneration;
        [SerializeField] private BaseItem ElememtSetArmor;
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
            if (_owner.Inventory.Armor?.NameItem == ElememtSetArmor.NameItem &&
                _owner.Inventory.Boot?.NameItem == ElememtSetBoot.NameItem)
            {
                FireCirlceSpell = CheckSkill();
                if (FireCirlceSpell == null)
                {
                    FireCirlceSpell = Instantiate(FireCirlcePrefab, _owner.gameObject.transform);
                    Debug.Log("�� ���� �����");
                }
                else
                {
                    Debug.Log("� ����� �� ��� ����");
                }
            }
            else
                Debug.Log("��� �� ����");
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