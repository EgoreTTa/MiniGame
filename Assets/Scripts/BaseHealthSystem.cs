namespace Assets.Scripts
{
    using GUI;
    using Interfaces;
    using Items;
    using NoMonoBehaviour;
    using UnityEngine;

    public abstract class BaseHealthSystem : MonoBehaviour
    {
        public abstract float Health { get; protected set; }
        public abstract float MinHealth { get; set; }
        public abstract float MaxHealth { get; set; }
        public abstract bool IsLive { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseHealthSystem"/>
        /// </summary>
        public abstract BaseHealthSystem Construct(ManagerGUI managerGUI = null);

        public abstract void TakeHealth(Health health);
        public abstract void TakeDamage(Damage damage);
        public abstract void Subscribe(IHealthChangeable healthChangeable);
        public abstract void Unsubscribe(IHealthChangeable healthChangeable);
    }
}