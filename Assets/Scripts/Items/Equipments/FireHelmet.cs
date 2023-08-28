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
        //      [SerializeField] private int Armor - пока под вопросом

        [SerializeField] private TypesEquipment _typeEquipment;

        public TypesEquipment TypeEquipment => _typeEquipment;
        private bool EquipBool = false;

        IEnumerator Regen()
        {
            yield return new WaitForSeconds(1);
            var health = new Health(_owner, gameObject, _changeRegeneration);
            (_owner as IHealthSystem).TakeHealth(health);
        }




        public void Equip()
        {
            _owner.MaxHealth += _changeMaxHealth;
            StartCoroutine(Regen());
           FireCircke.SetActive(true);
           FireCircke.transform.parent = _owner.transform;
            


        }

        public void Unequip()
        {
            _owner.MaxHealth -= _changeMaxHealth;
            StopCoroutine(Regen());
           FireCircke.SetActive(false);
        }
    }
}

