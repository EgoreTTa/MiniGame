namespace Mobs
{
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public abstract class BaseHealthSystem : MonoBehaviour
    {
        public abstract float Health { get; protected set; }
        public abstract float BaseMinHealth { get; }
        public abstract float MinHealth { get; set; }
        public abstract float BaseMaxHealth { get; }
        public abstract float MaxHealth { get; set; }
        public abstract bool IsLive { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseHealthSystem"/>
        /// </summary>
        public abstract BaseHealthSystem Construct(BaseMob owner = null);

        public abstract void TakeHealth(Health health);
        public abstract void TakeDamage(Damage damage);
        public abstract void Subscribe(ITakeDamage takeDamage);
        public abstract void Unsubscribe(ITakeDamage takeDamage);
        public abstract void Subscribe(ITakeHealth takeHealth);
        public abstract void Unsubscribe(ITakeHealth takeHealth);
    }
}