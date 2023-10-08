namespace Assets.Scripts.Items
{
    using UnityEngine;

    [RequireComponent(
        typeof(SpriteRenderer),
        typeof(Rigidbody2D),
        typeof(Collider2D))]
    public abstract class BaseItem : MonoBehaviour
    {
        protected Player _owner;
        protected SpriteRenderer _spriteRenderer;
        protected Rigidbody2D _rigidbody;
        protected Collider2D _collider;
        [SerializeField] protected string _nameItem;
        [SerializeField] protected string _description;
        [SerializeField] protected float _currency;

        public string NameItem => _nameItem;
        public string Description => _description;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public float Currency => _currency;

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