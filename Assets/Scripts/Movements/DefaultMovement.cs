namespace Assets.Scripts.Movements
{
    using Interfaces;
    using UnityEngine;

    public class DefaultMovement : MonoBehaviour, IMovementSystem
    {
        private bool _isConstruct;
        private Vector3 _direction = Vector3.up;
        private Transform _transform;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minMoveSpeed;
        [SerializeField] private float _maxMoveSpeed;

        public Vector3 Direction => _direction;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _moveSpeed = value;
            }
        }

        public float MinMoveSpeed
        {
            get => _minMoveSpeed;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _minMoveSpeed = value;
            }
        }

        public float MaxMoveSpeed
        {
            get => _maxMoveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                _maxMoveSpeed = value;
            }
        }

        public IMovementSystem Construct(Transform transform)
        {
            if (_isConstruct is false)
            {
                _transform = transform;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void Move(Vector3 vector)
        {
            _direction = vector.normalized;
            _transform.up = _direction;
            _transform.position += _transform.up * _moveSpeed * Time.deltaTime;
        }
    }
}