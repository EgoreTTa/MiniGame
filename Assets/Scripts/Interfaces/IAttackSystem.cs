namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IAttackSystem
    {
        public StatesOfAttack StateOfAttack { get; }

        public void Attack();
    }
}