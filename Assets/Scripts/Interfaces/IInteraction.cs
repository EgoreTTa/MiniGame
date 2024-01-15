namespace Interfaces
{
    using Mobs.Player;

    public interface IInteraction
    {
        public bool IsInteract { get; }

        public void Interact(Player player);
    }
}