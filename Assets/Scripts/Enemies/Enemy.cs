namespace Assets.Scripts.Enemies
{
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
            var direction = target.transform.position - transform.position;
            Walk(direction);
        }

        private void Walk(Vector3 vector)
        {
            // _direction = vector.normalized;
            // transform.up = _direction;
            // transform.position += _direction * _moveSpeed * Time.deltaTime;
        }
    }
}