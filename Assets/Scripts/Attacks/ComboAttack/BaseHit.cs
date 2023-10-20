namespace Assets.Scripts.Interfaces
{
    using Mobs;
    using Enums;
    using UnityEngine;

    public abstract class BaseHit : MonoBehaviour
    {
        public abstract float DamageCount { get; set; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseHit"/>
        /// </summary>
        public abstract BaseHit Construct(
            BaseMob owner,
            GroupsMobs ownerGroupMobs,
            BaseHealthSystem ownerHealthSystem);

        public abstract void Hit(float ownerDamageCount);
        public abstract void Recovery();
    }
}