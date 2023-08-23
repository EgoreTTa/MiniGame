namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IAttack
    {
        public StatesOfAttack StateOfAttack { get; }

        public void Attack();
    }
}