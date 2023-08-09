using UnityEngine;

[DisallowMultipleComponent]
public class DefaultHelmet : BaseItem, IEquipment
{
    [SerializeField] private TypesEquipment _typeEquipment;
    [SerializeField] private float _changeMaxHealth;
    [SerializeField] private BaseMob _parent;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    public TypesEquipment TypeEquipment => _typeEquipment;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public override void PickUp(BaseMob parent)
    {
        _parent = parent;
        _spriteRenderer.enabled = false;
        _rigidbody.simulated = false;
        _collider.enabled = false;
    }

    public void Equip() { _parent.MaxHealth += _changeMaxHealth; }

    public void Unequip() { _parent.MaxHealth -= _changeMaxHealth; }

    public override void PickDown(BaseMob parent)
    {
        transform.position = parent.transform.position;
        _parent = null;
        _spriteRenderer.enabled = true;
        _rigidbody.simulated = true;
        _collider.enabled = true;
    }
}