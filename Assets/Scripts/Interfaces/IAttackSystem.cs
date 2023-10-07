namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IAttackSystem
    {
        public StatesOfAttack StateOfAttack { get; }
        public float DamageCount { get; set; }

        public void Attack();
    }
}