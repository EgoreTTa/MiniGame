namespace Assets.Scripts.Skills
{
    using UnityEngine;

    [RequireComponent(
        typeof(SpriteRenderer),
        typeof(Rigidbody2D),
        typeof(Collider2D))]
    public abstract class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected string _nameItem;
        [SerializeField] protected string _description;
        [SerializeField] protected float _currency;
        [SerializeField] protected float _cooldown;
        protected Player _owner;
        protected SpriteRenderer _spriteRenderer;
        protected Rigidbody2D _rigidbody;
        protected Collider2D _collider;

        public string NameItem => _nameItem;
        public string Description => _description;
        public float Currency => _currency;
        public float Cooldown => _cooldown;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public virtual void PickUp(Player owner)
        {
            _owner = owner;
            _spriteRenderer.enabled = false;
            _rigidbody.simulated = false;
            _collider.enabled = false;
        }

        public virtual void PickDown(Player owner)
        {
            transform.position = owner.transform.position;
            _owner = null;
            _spriteRenderer.enabled = true;
            _rigidbody.simulated = true;
            _collider.enabled = true;
        }
    }
}