namespace Assets.Scripts.Items
{
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Vodka : BaseItem
    {
        [SerializeField] private float _heal;

        public override void PickUp(Player owner)
        {
            var health = new Health(owner, gameObject, _heal);
            (owner as IHealthSystem).TakeHealth(health);
           // Destroy(gameObject);
        }
    }
}