namespace Assets.Scripts.Enemies.RangerEnemy
{
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Bottle : MonoBehaviour, IProjectile
    {
        private float _speed;
        private Damage _damage;
        private BaseMob _owner;

        public Vector3 Direction
        {
            get => transform.up;
            set => transform.up = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public BaseMob Owner
        {
            get => _owner;
            set => _owner = value;
        }

        public Damage Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public void Launch(float speed, Damage damage, Vector3 direction, float timeFly, BaseMob owner)
        {
            _speed = speed;
            _damage = damage;
            transform.up = direction;
            _owner = owner;
            Destroy(this, timeFly);
            InvokeRepeating(nameof(Fly), 0, Time.fixedDeltaTime);
            gameObject.SetActive(true);
        }

        private void Fly()
        {
            transform.position += transform.up * _speed * Time.fixedDeltaTime;
        }

        private void Fall(Vector3 point)
        {
            transform.position = point;
            Destroy(this);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(Fly));
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
            {
                if (_owner!= null
                    &&
                    collider.gameObject != _owner.gameObject
                    &&
                    collider.gameObject.GetComponent<IHealthSystem>() is { } healthSystem)
                {
                    healthSystem.TakeDamage(_damage);
                    Fall(transform.position);
                }
            }
        }
    }
}