namespace Assets.Scripts.Interfaces
{
    using Enums;
    using UnityEngine;

    public interface IHit
    {
        public float DamageCount { get; set; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IJerk"/>
        /// </summary>
        public IHit Construct(
            IMob owner,
            GroupsMobs ownerGroupMobs,
            IHealthSystem ownerHealthSystem);

        public void Hit(float ownerDamageCount);
        public void Recovery();
    }
}