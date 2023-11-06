namespace Assets.Scripts.Explosion
{
    using Enums;
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Explosion : MonoBehaviour
    {
        private Damage _damage;
        private BaseMob _owner;
        private GroupsMobs _ownerGroupMobs;

        public void UpdateExplosion(float damageCount, BaseMob owner, GroupsMobs ownerGroupMobs)
        {
            _owner = owner;
            _ownerGroupMobs = ownerGroupMobs;
            _damage = new Damage(null, gameObject, damageCount, TypesDamage.Clear);
            Destroy(gameObject, Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
            {
                if (collider.GetComponent<BaseMob>() is { } mob
                    &&
                    mob != _owner
                    &&
                    mob.GroupMobs != _ownerGroupMobs
                   )
                {
                    if (collider.gameObject.GetComponent<BaseHealthSystem>() is { } healthSystem)
                        healthSystem.TakeDamage(_damage);
                }
            }
        }
    }
}