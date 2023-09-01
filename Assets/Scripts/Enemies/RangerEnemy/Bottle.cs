namespace Assets.Scripts.Enemies.RangerEnemy
{
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class Bottle : MonoBehaviour
    {
        [SerializeField] private float _timeFly;
        private float _speed;
        private Damage _damage;
        private GameObject _parent;

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

        public GameObject Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public Damage Damage
        {
            get => _damage;
            set => _damage = value;
        }

        private void Awake()
        {
            InvokeRepeating(nameof(Fly), 0, Time.fixedDeltaTime);
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
                if (collider.gameObject != _parent
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