namespace Assets.Scripts.Interfaces
{
    using NoMonoBehaviour;

    public interface IHealthSystem
    {
        public float Health { get; }
        public float MinHealth { get; set; }
        public float MaxHealth { get; set; }
        public bool IsLive { get; }

        public void TakeHealth(Health health);
        public void TakeDamage(Damage damage);
    }
}