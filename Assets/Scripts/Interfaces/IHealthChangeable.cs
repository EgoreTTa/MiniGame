namespace Assets.Scripts.Interfaces
{
    public interface IHealthChangeable
    {
        public void UpdateHealthSystem(BaseHealthSystem healthSystem);
        public void TakeHealth(BaseHealthSystem healthSystem);
        public void TakeDamage(BaseHealthSystem healthSystem);
    }
}