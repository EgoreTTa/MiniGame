namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IAbility
    {
        public StatesOfAbility StateOfAbility { get; }

        public void Cast();
    }
}