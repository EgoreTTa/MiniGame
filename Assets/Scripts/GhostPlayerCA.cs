namespace Assets.Scripts
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    [DisallowMultipleComponent]

    public class GhostPlayerCA: MonoBehaviour , IMob
    {
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] float _timeLife;
        [SerializeField] float _damageReduction;
        [SerializeField] float _speedAttack;

        private IHealthSystem _healthSystem;
        private IMoveSystem _moveSystem;
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
