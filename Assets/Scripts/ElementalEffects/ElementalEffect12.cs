﻿namespace Assets.Scripts.ElementalEffects
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class ElementalEffect12 : BaseElementalEffect
    {
        [SerializeField] private float _damageCount = 4f;
        [SerializeField] private float _timeOfAction = 15f;
        private float _timerOfAction;
        [SerializeField] private float _timeDamage = .1f;
        private float _timerDamage;
        [SerializeField] private float _changeMoveSpeed = 1.5f;

        private void Start()
        {
            _typeElement = TypesElement.ElementalEffect12;
            if (_target is IMoveSystem moveSystem)
                moveSystem.MoveSpeed -= _changeMoveSpeed;
            else
                Destroy(this);
        }

        private void Update()
        {
            if (_timerOfAction < _timeOfAction)
            {
                _timerOfAction += Time.deltaTime;
                if (_timerDamage < _timeDamage)
                {
                    _timerDamage += Time.deltaTime;
                }
                else
                {
                    _timerDamage -= _timeDamage;
                    MakeDamage();
                }
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (_target is IMoveSystem moveSystem)
                moveSystem.MoveSpeed += _changeMoveSpeed;
        }

        private void MakeDamage()
        {
            var damage = new Damage(
                _owner,
                null,
                _damageCount,
                TypesDamage.Clear);
            if (_target is IHealthSystem healthSystem)
                healthSystem.TakeDamage(damage);
        }
    }
}