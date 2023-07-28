using UnityEngine;

[DisallowMultipleComponent]
public class Vodka : BaseItem
{
    [SerializeField] private int _heal;

    public override void PickUp(BaseMob parent)
    {
        parent.Scorer.PickItem(this);
        parent.Health += _heal;
        Destroy(gameObject);
    }
}