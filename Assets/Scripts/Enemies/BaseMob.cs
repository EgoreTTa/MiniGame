namespace Assets.Scripts.Enemies
{
    using Enums;
    using UnityEngine;

    public abstract class BaseMob : MonoBehaviour
    {
        [SerializeField] protected float _health;
        [SerializeField] protected float _minHealth;
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected string _firstname;
        [SerializeField] protected float _damageCount;
        [SerializeField] protected float _viewRadius;
        [SerializeField] protected GroupsMobs _groupMobs;
        protected bool _isLive = true;

        public string Firstname => _firstname;
        public bool IsLive => _isLive;
        public GroupsMobs GroupMobs => _groupMobs;

        public float DamageCount
        {
            get => _damageCount;
            set
            {
                if (value < 0) value = 0;
                _damageCount = value;
            }
        }

        public float ViewRadius
        {
            get => _viewRadius;
            set
            {
                if (value < 0) value = 0;
                _viewRadius = value;
            }
        }
    }
}