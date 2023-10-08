namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IMob
    {
        public string FirstName { get; }
        public GroupsMobs GroupMobs { get; }
        public IHealthSystem HealthSystem { get; }
        public IMoveSystem MoveSystem { get; }
        public IAttackSystem AttackSystem { get; }
    }
}