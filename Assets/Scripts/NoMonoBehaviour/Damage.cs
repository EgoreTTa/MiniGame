namespace Assets.Scripts.NoMonoBehaviour
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class Damage
    {
        private readonly IMob _owner;
        private readonly GameObject _damageBy;
        private readonly float _countDamage;
        private readonly TypesDamage _typeDamage;

        public IMob Owner => _owner;
        public GameObject DamageBy => _damageBy;
        public float CountDamage => _countDamage;
        public TypesDamage TypeDamage => _typeDamage;

        public Damage(
            IMob owner,
            GameObject damageBy,
            float countDamage,
            TypesDamage typeDamage)
        {
            _owner = owner;
            _damageBy = damageBy;
            _countDamage = countDamage;
            _typeDamage = typeDamage;
        }
    }
}