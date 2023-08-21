public interface IHealthSystem
{
    public float Health { get; }
    public float MinHealth { get; set; }
    public float MaxHealth { get; set; }

    public void TikeHealth(Health health);
    public void TikeDamage(Damage damage);
}