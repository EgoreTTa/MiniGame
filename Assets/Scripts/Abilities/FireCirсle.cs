namespace Assets.Scripts.Abilities
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Abilities;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using Unity.Collections;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class FireCircle : DefaultAbility
    {
        [SerializeField] private float _damageCount;
        private List<IHealthSystem> _healthSystems = new();
        [SerializeField] private float _intervalDamaged;
        [SerializeField] private GameObject PlayerGameObject;
        [SerializeField] private bool _active;

        private Damage _damage;

        public bool Active
        {
            get => _active;
        }

        public bool ChangeActive
        {
            set => _active = value;
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
                if (!collider.GetComponent<Player>() &&
                    collider.GetComponent<BaseMob>().IsLive == true)

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
            if (_healthSystems is not null)
                foreach (var _healthSystems in _healthSystems)
                {
                    _healthSystems?.TakeDamage(_damage);
                }
        }
    }
}