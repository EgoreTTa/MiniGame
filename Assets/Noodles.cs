public class Noodles : BaseItem, IUsable
{
    public void Use(BaseMob target)
    {
        target.MaxStamina --;
        target.Stamina = target.MaxStamina;
        Destroy(gameObject);
    }

    public override void PickUp(BaseMob parent)
    {
        parent.Scorer.PickItem(this);
    }
}