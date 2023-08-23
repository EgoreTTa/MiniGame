namespace Assets.Scripts.Enemies.RangerEnemy
{
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    public class RangerEnemy : BaseMob
    {
        [SerializeField] private int _score;
        [SerializeField] private GameObject _targetToAttack;
        [SerializeField] private GameObject _bottle;
        [SerializeField] private float _bottleSpeed;
        [SerializeField] private float _rangeOfAttack;
        private readonly float _throwingCone = .1f;
        [SerializeField] private float _intervalAttack;
        private float _timerIntervalAttack;

        public GameObject TargetToAttack
        {
            get => _targetToAttack;
            set
            {
                if (value.activeSelf) _targetToAttack = value;
            }
        }

        private void Update()
        {
            if (_timerIntervalAttack < _intervalAttack) _timerIntervalAttack += Time.deltaTime;

            if (_targetToAttack != null
                &&
                _targetToAttack.activeSelf)
                ChoiceOfAction();
        }

        private void ChoiceOfAction()
        {
            var distance = Vector3.Distance(
                transform.position,
                _targetToAttack.transform.position);

            if (distance > _rangeOfAttack)
            {
                MoveToRangeOfAttack(_targetToAttack);
            }
            else
            {
                var direction = _targetToAttack.transform.position - transform.position;

                if (Vector3.Angle(direction, transform.up) > _throwingCone)
                {
                    var axis = Vector3.SignedAngle(
                        direction,
                        transform.up,
                        Vector3.back);
                    Rotate(axis);
                }
                else
                {
                    if (_timerIntervalAttack >= _intervalAttack)
                    {
                        _timerIntervalAttack -= _intervalAttack;
                        BottleThrow();
                    }
                }
            }
        }

        private void Rotate(float axis)
        {
            axis = axis > 0 ? 1 : -1;
            transform.up = Direction;
        }

        private void MoveToRangeOfAttack(GameObject target)
        {
            var direction = target.transform.position - transform.position;
            Walk(direction);
        }

        private void BottleThrow()
        {
            var directionThrow = transform.up.normalized;
            var bottle = Instantiate(_bottle, transform.position, Quaternion.identity).GetComponent<Bottle>();

            if (bottle != null)
            {
                var bottleDamage = new Damage(this, null, _damageCount, TypesDamage.Clear);
                bottle.Direction = directionThrow;
                bottle.Damage = bottleDamage;
                bottle.Speed = _bottleSpeed;
                bottle.Parent = gameObject;
                bottle.Distance = Vector3.Distance(
                    transform.position,
                    _targetToAttack.transform.position);
            }
        }
    }
}