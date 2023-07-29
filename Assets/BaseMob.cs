using UnityEngine;

public abstract class BaseMob : MonoBehaviour
{
    [Header("Здоровье")] 
    [SerializeField] protected float _health;
    [SerializeField] protected float _minHealth;
    [SerializeField] protected float _maxHealth;

    [Header("Выносливость")] 
    [SerializeField] protected int _stamina;
    [SerializeField] protected int _minStamina;
    [SerializeField] protected int _maxStamina;

    [Header("Скорость передвижения")] 
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _minMoveSpeed;
    [SerializeField] protected float _maxMoveSpeed;

    [Header("Остальные хар-ки")] 
    [SerializeField] protected string _firstname;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _turningSpeed;
    protected bool _live = true;
    private ScoreCounter _scorer = new();

    public float Health
    {
        get => _health;
        set
        {
            if (value < _health) DecreaseHealth();
            if (value < _minHealth)
            {
                value = _minHealth;
                _live = false;
                Destroy(gameObject);
            }

            if (value > _maxHealth)
            {
                value = _maxHealth;
            }

            _health = value;
        }
    }

    protected virtual void DecreaseHealth() { }

    public int Stamina
    {
        get => _stamina;
        set
        {
            if (value < _minStamina) value = _minStamina;
            if (value > _maxStamina) value = _maxStamina;
            _stamina = value;
        }
    }

    public int MinStamina
    {
        get => _minStamina;
        set
        {
            if (value <= 0) value = 1;
            if (value > _maxStamina) value = _maxStamina;
            _stamina = value;
        }
    }

    public int MaxStamina
    {
        get => _maxStamina;
        set
        {
            if (value <= _minStamina) value = _minStamina += 1;
            _maxStamina = value;
        }
    }

    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (value < _minMoveSpeed) value = _minMoveSpeed;
            if (value > _maxMoveSpeed) value = _maxMoveSpeed;
            _moveSpeed = value;
        }
    }

    public string Firstname => _firstname;

    public float Damage
    {
        get => _damage;
        set
        {
            if (value < 0) value = 0;
            _damage = value;
        }
    }

    public float ViewRadius
    {
        get => _viewRadius;
        set
        {
            if (value < 0) value = 0;
            _viewRadius = value;
        }
    }

    public float TurningSpeed
    {
        get => _turningSpeed;
        set
        {
            if (value < 0) value = 0;
            _turningSpeed = value;
        }
    }

    public bool Live => _live;
    public ScoreCounter Scorer => _scorer;

    protected virtual void Walk()
    {
        transform.position += transform.up.normalized * _moveSpeed * Time.deltaTime;
    }
    protected virtual void PickItem() { }

    protected virtual void Rotate(float axisX)
    {
        axisX = axisX > 0 ? 1 : -1;
        transform.Rotate(0f, 0f, _turningSpeed * Time.deltaTime * axisX);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<BaseItem>() is { } item)
        {
            PickItem();
            if (item.GetComponent<IUsable>() is { } usable)
            {
                usable.Use(this);
            }
        }
    }
}