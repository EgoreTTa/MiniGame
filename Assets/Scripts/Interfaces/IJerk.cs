namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IJerk
    {
        public StatesOfJerk StateOfJerk { get; }

        public void Jerk();
    }
}