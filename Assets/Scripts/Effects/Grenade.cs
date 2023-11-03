namespace Assets.Scripts.Grenade
{
    using Attacks.DefaultRangeAttack;
    using Explosion;
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Grenade : BaseProjectile
    {
        private bool _isConstruct;
        private float _speed;
        private Damage _damage;
        private GameObject _explosion;
        private BaseMob _owner;
        private float _timeExplosion;
        private float _damageCountExplosion;

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

        public override BaseProjectile Construct(float speed, Damage damage, Vector3 direction, float timeFly, BaseMob owner)
        {
            if (_isConstruct is false)
            {
                _speed = speed;
                _damage = damage;
                transform.up = direction;
                _owner = owner;
                InvokeRepeating(nameof(Fly), 0, Time.fixedDeltaTime);
                Invoke(nameof(ActionExplosion), timeFly);
                gameObject.SetActive(true);
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void SetExplosion(float timeExplosion, float damageCount, GameObject gameObjectExpolosion)
        {
            _timeExplosion = timeExplosion;
            _damageCountExplosion = damageCount;
            _explosion = gameObjectExpolosion;
        }

        private void Fly()
        {
            transform.position += transform.up * _speed * Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(Fly));
            CancelInvoke(nameof(ActionExplosion));
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
            {
                if (_owner != null
                    &&
                    collider.gameObject != _owner.gameObject)
                {
                    if (collider.gameObject.GetComponent<BaseHealthSystem>() is { } healthSystem)
                        healthSystem.TakeDamage(_damage);

                    ActionExplosion();
                }
            }
        }

        private void ActionExplosion()
        {
            var explosion = Instantiate(_explosion, gameObject.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().UpdateExplosion(_damageCountExplosion);
            Destroy(gameObject);
        }
    }
}