namespace Assets.Scripts.Attacks
{
    using System;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultAttack : MonoBehaviour, IAttack
    {
        [SerializeField] private float _timeSwing;
        private float _timerSwing;
        [SerializeField] private float _timeHitting;
        private float _timerHitting;
        [SerializeField] private float _timeRecovery;
        private float _timerRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        private BaseMob _owner;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception("Default not instance BaseMob");
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        public void Attack()
        {
            if (_stateOfAttack != StatesOfAttack.Idle) return;

            if (_owner.Stamina > 0)
            {
                _owner.Stamina--;
                _stateOfAttack = StatesOfAttack.Swing;
            }
        }

        private void Hitting()
        {
            if (_timerHitting <= _timeHitting)
            {
                _timerHitting += Time.deltaTime;
            }
        }

        private void Update()
        {
            if (_stateOfAttack == StatesOfAttack.Idle) return;
            ActionChoice();
        }

        private void ActionChoice()
        {
            switch (_stateOfAttack)
            {
                case StatesOfAttack.Idle:
                    break;
                case StatesOfAttack.Swing:
                    if (_timerSwing > _timeSwing)
                    {
                        _spriteRenderer.enabled = true;
                        _circleCollider.enabled = true;
                        _timerSwing -= _timeSwing;
                        _stateOfAttack = StatesOfAttack.Hitting;
                        return;
                    }

                    Swing();
                    break;
                case StatesOfAttack.Hitting:
                    if (_timerHitting > _timeHitting)
                    {
                        _spriteRenderer.enabled = false;
                        _circleCollider.enabled = false;
                        _timerHitting -= _timeHitting;
                        _stateOfAttack = StatesOfAttack.Recovery;
                        return;
                    }

                    Hitting();
                    break;
                case StatesOfAttack.Recovery:
                    if (_timerRecovery > _timeRecovery)
                    {
                        _timerRecovery -= _timeRecovery;
                        _stateOfAttack = StatesOfAttack.Idle;
                        return;
                    }

                    Recovery();
                    break;
                default:
                    throw new Exception("FSM of DefaultAttack: not valid state");
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<IHealthSystem>() is { } healthSystem)
            {
                if (collider.gameObject != _owner.gameObject)
                {
                    var damage = new Damage(_owner, null, _owner.DamageCount, TypesDamage.Clear);
                    healthSystem.TakeDamage(damage);
                }
            }
        }
    }
}