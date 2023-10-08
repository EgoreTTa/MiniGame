namespace Assets.Scripts.Items
{
    using Interfaces;
    using JetBrains.Annotations;
    using NoMonoBehaviour;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class Vodka : BaseItem
    {
        [SerializeField] private float _heal;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            var health = new Health(owner, gameObject, _heal);
            owner.GetComponent<IHealthSystem>().TakeHealth(health);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _owner?.Inventory.Take(this);
        }
    }
}