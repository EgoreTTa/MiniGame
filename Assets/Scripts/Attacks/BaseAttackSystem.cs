namespace Assets.Scripts.Attacks
{
    using Mobs;
    using Enums;
    using UnityEngine;

    public abstract class BaseAttackSystem : MonoBehaviour
    {
        public abstract StatesOfAttack StateOfAttack { get; }
        public abstract float DamageCount { get; set; }
        public abstract float DamagePercent { get; set; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseAttackSystem"/>
        /// </summary>
        public abstract BaseAttackSystem Construct(
            BaseMob owner,
            GroupsMobs ownerGroupsMobs,
            GameObject ownerGameObject,
            BaseHealthSystem ownerHealthSystem,
            Transform ownerTransform);

        public abstract void Attack();
    }
}