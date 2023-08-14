public interface IInteraction
{
    public string FirstName { get; }

    public bool IsInteract { get; }

    void Interact(BaseMob mob);

}