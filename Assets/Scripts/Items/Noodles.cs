namespace Assets.Scripts.Items
{
    using Interfaces;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class Noodles : BaseItem, IUsable
    {
        public void Use(Player player) { }
    }
}