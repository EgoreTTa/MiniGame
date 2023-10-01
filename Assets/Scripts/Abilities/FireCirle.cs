namespace Assets.Scripts.Skills
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using Unity.Collections;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class FireCirle : BaseSkill
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
            set => _active = value;
        }

        void Update()
        {
            transform.position = transform.parent.position;
        }

        void Awake()
        {
            _damage = new Damage(null, gameObject, _damageCount, TypesDamage.Clear);
            InvokeRepeating(nameof(Damaged), 0f, _intervalDamaged);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
            {
                if (!collider.GetComponent<Player>())

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
            for (var i = _healthSystems.Count - 1; i >= 0; i--)
                _healthSystems[i]?.TakeDamage(_damage);
        }
    }
}