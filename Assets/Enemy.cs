using UnityEngine;

[DisallowMultipleComponent]
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
        var direction = _targetToAttack.transform.position - transform.position;
        var axis = Vector3.SignedAngle(
            direction,
            transform.up,
            Vector3.back);
        Rotate(axis);
        Walk();
    }
}