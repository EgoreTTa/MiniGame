namespace Assets.Scripts.Interfaces
{
    public interface IInteraction
    {
        public bool IsInteract { get; }

        public void Interact(Player player);
    }
}