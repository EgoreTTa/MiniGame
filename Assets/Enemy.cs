using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : BaseMob
{
    [SerializeField] private int _score;
    [SerializeField] private BaseMob _targetToAttack;
    [SerializeField] private BaseItem _targetToPickUp;
    [SerializeField] private Vector3? _targetToExplore;

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
            var distanceToMob = Vector3.Distance(
                _targetToPickUp.transform.position,
                transform.position);
            if (distanceToMob > _viewRadius)
            {
                _targetToAttack = null;
                return;
            }

            if (_health > _maxHealth / 100 * 20)
            {
                MoveToPosition(_targetToAttack.transform.position);
                return;
            }

            Retreat();
            return;
        }

        if (_targetToPickUp != null
            &&
            _targetToPickUp.gameObject.activeSelf)
        {
            var distanceToItem = Vector3.Distance(
                _targetToPickUp.transform.position, 
                transform.position);
            if (distanceToItem > _viewRadius)
            {
                _targetToPickUp = null;
                return;
            }
            MoveToPosition(_targetToPickUp.transform.position);
            return;
        }

        var target = LookAround();
        if (target is false)
        {
            if (_targetToExplore == null) FindPositionToExplore();
            if (_targetToExplore != null) MoveToPosition(_targetToExplore.Value);
        }
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        var axis = Vector3.SignedAngle(
            direction,
            transform.up,
            Vector3.back);
        Rotate(axis);
        Walk();
    }

    private bool LookAround()
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
            return true;
        }

        var items = casted.Where(x => x.transform.GetComponent<BaseItem>())
            .Distinct()
            .Select(x => x.transform.GetComponent<BaseItem>())
            .ToArray();
        if (items.Length > 0)
        {
            TargetToPickUp = items.First();
            return true;
        }
        return false;
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

    private void FindPositionToExplore()
    {
        _targetToExplore = new Vector3(
            Random.Range(-5, 5),
            Random.Range(-5, 5));
    }
}