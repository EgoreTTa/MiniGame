using UnityEngine;

public class Enemy : BaseMob
{
    [SerializeField] private int _score;
    [SerializeField] private GameObject _targetToAttack;

    public GameObject TargetToAttack
    {
        get => _targetToAttack;
        set
        {
            if (value.activeSelf)
            {
                _targetToAttack = value;
            }
        }
    }
    
    private void Update()
    {
        if (_targetToAttack != null
            &&
            _targetToAttack.activeSelf)
        {
            MoveToTarget(_targetToAttack);
        }
    }

    private void MoveToTarget(GameObject target)
    {
        var axis = Vector3.Angle(
            target.transform.position - transform.position,
            transform.right);
        axis -= 90;
        Rotate(axis);
        Walk();
    }
}