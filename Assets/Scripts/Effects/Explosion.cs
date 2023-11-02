namespace Assets.Scripts.Explosion
{
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Explosion : MonoBehaviour
    {
        private Damage _damage;
        private float _timeExplosion;
        private float _damageCount;

        public void UpdateExplosion(float timeExplosion, float _damageCount)
        {
            _damage = new Damage(null, gameObject, _damageCount, TypesDamage.Clear);
            Invoke(nameof(ActionExplosion), timeExplosion);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<BaseHealthSystem>() is { } healthSystem)
                healthSystem.TakeDamage(_damage);
        }

        private void ActionExplosion()
        {
            Destroy(gameObject);
        }
    }
}