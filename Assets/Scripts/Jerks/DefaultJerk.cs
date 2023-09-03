namespace Assets.Scripts.Jerks
{
    using Enemies;
    using Enums;
    using Interfaces;
    using UnityEngine;

    [RequireComponent(typeof(BaseMob))]
    public class DefaultJerk : MonoBehaviour, IJerk
    {
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeMoving;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private float _speedMoving;
        [SerializeField] private StatesOfJerk _stateOfJerk;
        private int _onlyDynamic = 1024;
        private int _nothing = 0;
        private BaseMob _owner;
        private IMoveSystem _moveSystem;

        public StatesOfJerk StateOfJerk => _stateOfJerk;

        private void Awake()
        {
            _owner = GetComponent<BaseMob>();
            _moveSystem = GetComponent<IMoveSystem>();
        }

        public void Jerk()
        {
            if (_stateOfJerk != StatesOfJerk.Idle) return;

            _stateOfJerk = StatesOfJerk.Swing;
            Invoke(nameof(IntoMoving), _timeSwing);
        }

        private void IntoMoving()
        {
            _stateOfJerk = StatesOfJerk.Moving;
            Invoke(nameof(IntoRecovery), _timeMoving);
            _owner.GetComponent<Rigidbody2D>().excludeLayers = _onlyDynamic;
            InvokeRepeating(nameof(Move), 0, Time.fixedDeltaTime);
        }

        private void Move()
        {
            _owner.transform.position += _moveSystem.Direction * _speedMoving * Time.fixedDeltaTime;
        }

        private void IntoRecovery()
        {
            _stateOfJerk = StatesOfJerk.Recovery;
            _owner.GetComponent<Rigidbody2D>().excludeLayers = _nothing;
            CancelInvoke(nameof(Move));
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfJerk = StatesOfJerk.Idle;
        }
    }
}