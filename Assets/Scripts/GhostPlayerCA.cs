namespace Assets.Scripts
{
    using NoMonoBehaviour;
    using System;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using Items;
    using UnityEngine.SceneManagement;

    [DisallowMultipleComponent]

    public class GhostPlayerCA: MonoBehaviour , IMob
    {
        public enum StatesOfPlayer
        {
            Idle,
            Move,
            Jerk,
            Attack,
            Interaction,
        }
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] float _timeLife;
        [SerializeField] float _damageReduction;
        [SerializeField] float _speedAttack;

        private IHealthSystem _healthSystem;
        private IMoveSystem _moveSystem;

        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;


        private IAttackSystem _attackSystem;
        public IAttackSystem AttackSystem => _attackSystem;
        public string FirstName => _firstname;
        public GroupsMobs GroupMobs => _groupMobs;
        public IHealthSystem HealthSystem => _healthSystem;
        public IMoveSystem MoveSystem => _moveSystem;

        private void Awake()
        {
            if (GetComponentInChildren<IAttackSystem>() is { } attackSystem) _attackSystem = attackSystem;
        }

        void Start()
        {
            InvokeRepeating(nameof(AttackCombo), 0f, _speedAttack);
            Invoke(nameof(Ending), _timeLife);
        }

        void AttackCombo()
        {
            _attackSystem.Attack();
        }

        void Ending()
        {
            Destroy(gameObject);
        }

    }
}
