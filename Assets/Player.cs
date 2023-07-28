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
                        enemy.Health -= _damage;
                        if (enemy.Live is false)
                        {
                            Scorer.KilledEnemy(enemy);
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
                _damage *= _increaseCharacteristicsOnMurder;
                break;
            case 2:
                _moveSpeed *= _increaseCharacteristicsOnMurder;
                break;
            default: break;
        }
    }

    private void Update()
    {
        var axisX = 0;
        if (Input.GetKey(KeyCode.A)) axisX++;
        if (Input.GetKey(KeyCode.D)) axisX--;

        if (Input.GetKey(KeyCode.W))
        {
            Walk();
        }

        if (axisX is not 0)
        {
            Rotate(axisX);
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