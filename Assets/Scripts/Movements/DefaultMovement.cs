namespace Assets.Scripts.Movements
{
    using Interfaces;
    using UnityEngine;

    public class DefaultMovement : MonoBehaviour, IMoveSystem
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minMoveSpeed;
        [SerializeField] private float _maxMoveSpeed;
        private Vector3 _direction = Vector3.up;

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

        public void Move(Vector3 vector)
        {
            _direction = vector.normalized;
            transform.up = _direction;
            transform.position += transform.up * _moveSpeed * Time.deltaTime;
        }
    }
}