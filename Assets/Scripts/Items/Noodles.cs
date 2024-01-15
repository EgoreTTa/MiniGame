namespace Items
{
    using Interfaces;
    using JetBrains.Annotations;
    using Mobs.Player;

    [UsedImplicitly]
    public class Noodles : BaseItem, IUsable
    {
        public void Use(Player player) { }
    }
}