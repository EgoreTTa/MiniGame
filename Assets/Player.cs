using UnityEngine;

public class Player : BaseMob
{
    private BumStats _stats = new();
    [SerializeField] private float _increaseCharacteristicsOnMurder = 1.1f;

    public BumStats Stats => _stats;

    public void Attack(Player target)
    {
        target.Health -= _damage;
        if (target.Live is false)
        {
            Stats.KilledEnemy();
            IncreaseCharacteristics();
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
    }

    protected override void DecreaseHealth()
    {
        Stats.ClearStreak();
    }

    protected override void PickItem()
    {
        Stats.PickItem();
    }
}