namespace Assets.Scripts.Jerks
{
    using Enums;
    using Mobs;
    using Movements;
    using UnityEngine;

    public class DefaultJerk : BaseJerk
    {
        private bool _isConstruct;
        private readonly int _onlyDynamic = 1024;
        private readonly int _nothing = 0;
        private Rigidbody2D _rigidbody2D;
        private BaseMovement _movementSystem;
        private Transform _transform;
        private Vector3 _direction;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeMoving;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private float _speedMoving;
        [SerializeField] private StatesOfJerk _stateOfJerk;

        public override StatesOfJerk StateOfJerk => _stateOfJerk;

        public override BaseJerk Construct(
            BaseMob owner,
            Transform ownerTransform,
            Rigidbody2D ownerRigidbody2D,
            BaseMovement ownerMovementSystem)
        {
            if (_isConstruct is false)
            {
                _transform = ownerTransform;
                _rigidbody2D = ownerRigidbody2D;
                _movementSystem = ownerMovementSystem;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public override void Jerk(Vector3 direction)
        {
            if (_stateOfJerk != StatesOfJerk.Idle) return;

            _direction = direction;
            _stateOfJerk = StatesOfJerk.Swing;
            Invoke(nameof(IntoMoving), _timeSwing);
        }

        private void IntoMoving()
        {
            _stateOfJerk = StatesOfJerk.Moving;
            Invoke(nameof(IntoRecovery), _timeMoving);
            _rigidbody2D.excludeLayers = _onlyDynamic;
            InvokeRepeating(nameof(Move), 0, Time.fixedDeltaTime);
        }

        private void Move()
        {
            _rigidbody2D.MovePosition(_transform.position + _direction * (_speedMoving * Time.fixedDeltaTime));
        }

        private void IntoRecovery()
        {
            _stateOfJerk = StatesOfJerk.Recovery;
            _rigidbody2D.excludeLayers = _nothing;
            CancelInvoke(nameof(Move));
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfJerk = StatesOfJerk.Idle;
        }
    }
}