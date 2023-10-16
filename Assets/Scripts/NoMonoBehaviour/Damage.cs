namespace Assets.Scripts.NoMonoBehaviour
{
    using Mobs;
    using Enums;
    using UnityEngine;

    public readonly struct Damage
    {
        private readonly BaseMob _owner;
        private readonly GameObject _damageBy;
        private readonly float _countDamage;
        private readonly TypesDamage _typeDamage;

        public BaseMob Owner => _owner;
        public GameObject DamageBy => _damageBy;
        public float CountDamage => _countDamage;
        public TypesDamage TypeDamage => _typeDamage;

        public Damage(
            BaseMob owner,
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