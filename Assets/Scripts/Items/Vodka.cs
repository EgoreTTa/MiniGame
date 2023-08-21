using UnityEngine;

[DisallowMultipleComponent]
public class Vodka : BaseItem
{
    [SerializeField] private float _heal;

    public override void PickUp(BaseMob parent)
    {
        var health = new Health(parent, gameObject, _heal);
        parent.Scorer.PickItem(this);
        (parent as IHealthSystem).TakeHealth(health);
        Destroy(gameObject);
    }
}