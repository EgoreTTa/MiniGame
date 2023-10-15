namespace Assets.Scripts.Interfaces
{
    using Enums;
    using UnityEngine;

    public interface IAttackSystem
    {
        public StatesOfAttack StateOfAttack { get; }
        public float DamageCount { get; set; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IAttackSystem"/>
        /// </summary>
        public IAttackSystem Construct(
            IMob owner,
            GroupsMobs ownerGroupsMobs,
            IHealthSystem ownerHealthSystem,
            Transform ownerTransform);

        public void Attack();
    }
}