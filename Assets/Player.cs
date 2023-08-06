using UnityEngine;

[DisallowMultipleComponent]
public class Player : BaseMob
{
    [SerializeField] private float _increaseCharacteristicsOnMurder;
    [SerializeField] private float _intervalForAttack;
    [SerializeField] private CircleCollider2D _triggerForAttack;
    [SerializeField] private float _offsetForAttack;
    private float _timerIntervalForAttack;
    private Inventory _inventory;

    public void Attack()
    {
        if (_stamina > 0)
        {
            var triggerPosition =
                transform.position +
                transform.up * _offsetForAttack;
            var casted = Physics2D.CircleCastAll(
                triggerPosition,
                _triggerForAttack.radius,
                Vector2.zero);
            if (casted.Length > 0)
            {
                foreach (var hit in casted)
                {
                    if (hit.collider.GetComponent<BaseMob>() is { } enemy
                        &&
                        enemy != this)
                    {
                        var damage = new Damage(this, null, TypeDamage.Clear, _damageCount);
                        enemy.TakeDamage(damage);
                        if (enemy.Live is false)
                        {
                            IncreaseCharacteristics();
                        }

                        break;
                    }
                }
            }

            _stamina--;
        }
    }

    private void IncreaseCharacteristics()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                _health *= _increaseCharacteristicsOnMurder;
                break;
            case 1:
                _damageCount *= _increaseCharacteristicsOnMurder;
                break;
            case 2:
                _moveSpeed *= _increaseCharacteristicsOnMurder;
                break;
            default: break;
        }
    }

    private void Update()
    {
        var axis = Vector3.zero;
        if (Input.GetKey(KeyCode.D)) axis.x++;
        if (Input.GetKey(KeyCode.A)) axis.x--;
        if (Input.GetKey(KeyCode.W)) axis.y++;
        if (Input.GetKey(KeyCode.S)) axis.y--;
        
        if (axis != Vector3.zero)
        {
            Walk(axis);
        }

        if (_timerIntervalForAttack <= _intervalForAttack)
        {
            _timerIntervalForAttack += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E)
            &&
            _timerIntervalForAttack > _intervalForAttack)
        {
            _timerIntervalForAttack -= _intervalForAttack;
            Attack();
        }
    }

    protected override void DecreaseHealth()
    {
        Scorer.ClearStreak();
    }
}