namespace Assets.Scripts.Interfaces
{
    using GUI;
    using NoMonoBehaviour;
    using UnityEngine;

    public interface IHealthSystem
    {
        public float Health { get; }
        public float MinHealth { get; set; }
        public float MaxHealth { get; set; }
        public bool IsLive { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IHealthSystem"/>
        /// </summary>
        public IHealthSystem Construct(ManagerGUI managerGUI = null);

        public void TakeHealth(Health health);
        public void TakeDamage(Damage damage);
    }
}