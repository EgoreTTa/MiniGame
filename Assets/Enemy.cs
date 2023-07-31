using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : BaseMob
{
    [SerializeField] private int _score;
    [SerializeField] private BaseMob _targetToAttack;
    [SerializeField] private BaseItem _targetToPickUp;

    public BaseMob TargetToAttack
    {
        get => _targetToAttack;
        set
        {
            if (value.gameObject.activeSelf)
            {
                _targetToAttack = value;
            }
        }
    }

    public BaseItem TargetToPickUp
    {
        get => _targetToPickUp;
        set
        {
            if (value.gameObject.activeSelf)
            {
                _targetToPickUp = value;
            }
        }
    }

    private void Update()
    {
        if (_targetToAttack != null
            &&
            _targetToAttack.gameObject.activeSelf)
        {
            if (_health <= _maxHealth / 100 * 20)
            {
                MoveToTarget(_targetToAttack.gameObject);
                return;
            }

            Retreat();
        }

        if (_targetToPickUp != null
            &&
            _targetToPickUp.gameObject.activeSelf)
        {
            MoveToTarget(_targetToPickUp.gameObject);
        }
    }

    private void MoveToTarget(GameObject target)
    {
        var direction = _targetToAttack.transform.position - transform.position;
        var axis = Vector3.SignedAngle(
            direction,
            transform.up,
            Vector3.back);
        Rotate(axis);
        Walk();
    }

    private void LookAround()
    {
        var casted = Physics2D.CircleCastAll(
            transform.position,
            _viewRadius,
            Vector2.zero);
        var mobs = casted
            .Where(x => x.transform.GetComponent<BaseMob>()
                        &&
                        x.transform.GetComponent<BaseMob>() != this)
            .Distinct()
            .Select(x => x.transform.GetComponent<BaseMob>())
            .ToArray();
        if (mobs.Length > 0)
        {
            TargetToAttack = mobs.First();
            return;
        }

        var items = casted.Where(x => x.transform.GetComponent<BaseItem>())
            .Distinct()
            .Select(x => x.transform.GetComponent<BaseItem>())
            .ToArray();
        if (items.Length > 0)
        {
            TargetToPickUp = items.First();
        }
    }

    private void Retreat()
    {
        var direction = _targetToAttack.transform.position - transform.position;
        var axis = Vector3.SignedAngle(
            direction,
            transform.up,
            Vector3.back);
        Rotate(-axis);
        Walk();
    }
}