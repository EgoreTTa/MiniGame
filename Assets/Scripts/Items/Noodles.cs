namespace Assets.Scripts.Items
{
    using Interfaces;

    public class Noodles : BaseItem, IUsable
    {
        public void Use(Player player) { }

        public new void PickUp(Player owner)
        {
            base.PickUp(owner);
        }
    }
}