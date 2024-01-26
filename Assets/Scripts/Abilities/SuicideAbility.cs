namespace Abilities
{
    using System.Collections.Generic;
    using Enums;
    using GUI;
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    public class SuicideAbility : BaseAbility
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GroupsMobs _ownerGroupMobs;
        private readonly List<BaseMob> _mobs = new();
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] [Min(.2f)] private float _timeToSwing;
        [SerializeField] [Min(.2f)] private float _timeToCasted;
        [SerializeField] [Min(.2f)] private float _timeToRecovery;
        [SerializeField] private float _radius;
        [SerializeField] private float _speedUse;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _power;
        [SerializeField] private float _duration;
        [SerializeField] private float _range;

        public override float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        public override float SpeedUse
        {
            get => _speedUse;
            set => _speedUse = value;
        }

        public override float Cooldown
        {
            get => _cooldown;
            set => _cooldown = value;
        }

        public override float Power
        {
            get => _power;
            set => _power = value;
        }

        public override float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public override float Range
        {
            get => _range;
            set => _range = value;
        }

        public override BaseAbility Construct(
            BaseMob owner, 
            GroupsMobs ownerGroupMobs, 
            GameObject ownerGameObject, 
            ManagerGUI managerGUI)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGroupMobs = ownerGroupMobs;
                _isConstruct = true;
                _managerGUI = null;
                return this;
            }

            return null;
        }

        private void IntoCasted()
        {
            _stateOfAbility = StatesOfAbility.Casted;

            var damageCount = _owner.HealthSystem.Health / 2;
            var damage = new Damage(_owner, gameObject, damageCount, TypesDamage.Clear);
            foreach (var mob in _mobs) mob.HealthSystem.TakeDamage(damage);
            damage = new Damage(_owner, gameObject, _owner.HealthSystem.MaxHealth, TypesDamage.Clear);
            _owner.HealthSystem.TakeDamage(damage);

            Invoke(nameof(IntoRecovery), _timeToCasted);
        }

        private void IntoRecovery()
        {
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            _stateOfAbility = StatesOfAbility.Recovery;
            Invoke(nameof(IntoStandby), _timeToRecovery);
        }

        private void IntoStandby()
        {
            _stateOfAbility = StatesOfAbility.Standby;
        }

        private void IntoSwing()
        {
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
            _stateOfAbility = StatesOfAbility.Swing;
            Invoke(nameof(IntoCasted), _timeToSwing);
        }

        public override void Cast()
        {
            IntoSwing();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<BaseMob>() is { } mob
                &&
                mob.GroupMobs != _ownerGroupMobs)
            {
                _mobs.Add(mob);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<BaseMob>() is { } mob
                &&
                _mobs.Contains(mob))
            {
                _mobs.Remove(mob);
            }
        }
    }
}