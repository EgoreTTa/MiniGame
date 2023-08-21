using UnityEngine;

public class Bottle : MonoBehaviour
{
    private Vector3 _direction;
    private float _distance;
    private float _speed;
    private Damage _damage;
    private GameObject _parent;

    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
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

    public float Distance
    {
        get => _distance;
        set => _distance = value;
    }

    private void Update()
    {
        var averageSpeed = _speed * Time.deltaTime;
        if (_distance > averageSpeed)
        {
            Fly(averageSpeed);
        }
        else
        {
            Fall(transform.position + _direction * averageSpeed);
        }
    }

    private void Fly(float averageSpeed)
    {
        transform.position += _direction * averageSpeed;
        _distance -= averageSpeed;
    }

    private void Fall(Vector3 point)
    {
        transform.position = point;
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
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