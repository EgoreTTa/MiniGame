namespace Movements
{
    using UnityEngine;

    public class DefaultMovement : BaseMovement
    {
        private bool _isConstruct;
        private Vector3 _direction = Vector3.up;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minMoveSpeed;
        [SerializeField] private float _maxMoveSpeed;

        public override Vector3 Direction => _direction;

        public override float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _moveSpeed = value;
            }
        }

        public override float MinMoveSpeed
        {
            get => _minMoveSpeed;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _minMoveSpeed = value;
            }
        }

        public override float MaxMoveSpeed
        {
            get => _maxMoveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                _maxMoveSpeed = value;
            }
        }

        public override BaseMovement Construct(Transform transform, Rigidbody2D rigidbody)
        {
            if (_isConstruct is false)
            {
                _transform = transform;
                _rigidbody = rigidbody;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        private void FixedUpdate()
        {
            if (_direction == default) return;
            _rigidbody.MovePosition(_transform.position + _direction * (_moveSpeed * Time.fixedDeltaTime));
            _transform.up = _direction;
            _direction = default;
        }

        public override void Move(Vector3 vector)
        {
            _direction = vector.normalized;
        }
    }
}