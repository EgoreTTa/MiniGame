namespace Assets.Scripts.Effects
{
    using System.Collections.Generic;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class FireCircle : MonoBehaviour, IEffect
    {
        [SerializeField] private float _damageCount;
        private List<IHealthSystem> _healthSystems = new();
        [SerializeField] private float _intervalDamaged;
        private Damage _damage;
        [SerializeField] private bool _isActive;

        public string EffectName => nameof(FireCircle);

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        private void Update()
        {
            transform.position = transform.parent.position;
        }

        private void Awake()
        {
            _damage = new Damage(null, gameObject, _damageCount, TypesDamage.Clear);
            InvokeRepeating(nameof(Damaged), 0f, _intervalDamaged);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
            {
                if (collider.GetComponent<Player>() is not null
                    &&
                    collider.GetComponent<BaseMob>().IsLive is true)

                    if (collider.gameObject.GetComponent<IHealthSystem>() is { } healthSystem)
                    {
                        _healthSystems.Add(healthSystem);
                    }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
                if (collider.GetComponent<IHealthSystem>() is { } healthSystem)
                    _healthSystems.Remove(healthSystem);
        }

        private void Damaged()
        {
            foreach (var _healthSystems in _healthSystems)
            {
                _healthSystems?.TakeDamage(_damage);
            }
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}