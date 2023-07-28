using UnityEngine;

public class Vodka : BaseItem
{
    [SerializeField] private int _heal;

    public override void Use(BaseMob target)
    {
        target.Health += _heal;
        Destroy(gameObject);
    }
}