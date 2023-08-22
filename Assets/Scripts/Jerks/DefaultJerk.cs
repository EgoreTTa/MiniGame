namespace Assets.Scripts.Jerks
{
    using System;
    using Enemies;
    using Enums;
    using Interfaces;
    using UnityEngine;

    [RequireComponent(typeof(BaseMob))]
    public class DefaultJerk : MonoBehaviour, IJerk
    {
        [SerializeField] private float _timeSwing;
        private float _timerSwing;
        [SerializeField] private float _timeMoving;
        private float _timerMoving;
        [SerializeField] private float _timeRecovery;
        private float _timerRecovery;
        [SerializeField] private float _speedMoving;
        [SerializeField] private StatesOfJerk _stateOfJerk;
        private int _onlyDynamic = 1024;
        private int _nothing = 0;
        private BaseMob _owner;

        public StatesOfJerk StateOfJerk => _stateOfJerk;

        private void Awake()
        {
            _owner = GetComponent<BaseMob>();
        }

        private void Moving()
        {
            if (_timerMoving <= _timeMoving)
            {
                _timerMoving += Time.deltaTime;
                Move();
            }
        }

        private void Move()
        {
            _owner.transform.position += _owner.Direction * _speedMoving * Time.deltaTime;
        }

        private void Update()
        {
            if (_stateOfJerk == StatesOfJerk.Idle) return;
            ActionChoice();
        }

        private void ActionChoice()
        {
            switch (_stateOfJerk)
            {
                case StatesOfJerk.Idle:
                    break;
                case StatesOfJerk.Swing:
                    if (_timerSwing > _timeSwing)
                    {
                        _timerSwing -= _timeSwing;
                        _owner.GetComponent<Rigidbody2D>().excludeLayers = _onlyDynamic;
                        _stateOfJerk = StatesOfJerk.Moving;
                        return;
                    }

                    Swing();
                    break;
                case StatesOfJerk.Moving:
                    if (_timerMoving > _timeMoving)
                    {
                        _timerMoving -= _timeMoving;
                        _owner.GetComponent<Rigidbody2D>().excludeLayers = _nothing;
                        _stateOfJerk = StatesOfJerk.Recovery;
                        return;
                    }

                    Moving();
                    break;
                case StatesOfJerk.Recovery:
                    if (_timerRecovery > _timeRecovery)
                    {
                        _timerRecovery -= _timeRecovery;
                        _stateOfJerk = StatesOfJerk.Idle;
                        return;
                    }

                    Recovery();
                    break;
                default:
                    throw new Exception("FSM of DefaultJerk: not valid state");
            }
        }

        private void Recovery()
        {
            if (_timerRecovery <= _timeRecovery) _timerRecovery += Time.deltaTime;
        }

        private void Swing()
        {
            if (_timerSwing <= _timeSwing) _timerSwing += Time.deltaTime;
        }

        public void Jerk()
        {
            if (_stateOfJerk != StatesOfJerk.Idle) return;

            _stateOfJerk = StatesOfJerk.Swing;
        }
    }
}