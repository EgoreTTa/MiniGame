public class Noodles : BaseItem
{
    public override void Use(BaseMob target)
    {
        target.MaxStamina --;
        target.Stamina = target.MaxStamina;
        Destroy(gameObject);
    }
}