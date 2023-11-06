namespace Assets.Scripts.Explosion
{
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Explosion : MonoBehaviour
    {
        private Damage _damage;

        public void UpdateExplosion(float damageCount)
        {
            _damage = new Damage(null, gameObject, damageCount, TypesDamage.Clear);
            Destroy(gameObject, Time.fixedTime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<BaseHealthSystem>() is { } healthSystem)
                healthSystem.TakeDamage(_damage);
        }
    }
}