namespace Assets.Scripts.Interfaces
{
    using Items;
    using NoMonoBehaviour;

    public interface ITrading
    {
        public void Trade(BaseItem item, Inventory inventory);
    }
}