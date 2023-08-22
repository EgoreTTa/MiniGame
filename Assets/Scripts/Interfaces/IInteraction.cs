namespace Assets.Scripts.Interfaces
{
    using Enemies;

    public interface IInteraction
    {
        public string FirstName { get; }

        public bool IsInteract { get; }

        public void Interact(BaseMob mob);
    }
}