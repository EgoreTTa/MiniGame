﻿namespace Assets.Scripts.ElementalEffects
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class ElementalEffect1 : BaseElementalEffect
    {
        [SerializeField] private float _damageCount = 2f;
        [SerializeField] private float _timeOfAction = 10f;
        [SerializeField] private float _timeDamage = .2f;

        private void Start()
        {
            _typeElement = TypesElement.ElementalEffect1;
            CombineEffect();
            Destroy(this, _timeOfAction);
            InvokeRepeating(nameof(MakeDamage), 0, _timeDamage);
        }

        private void MakeDamage()
        {
            var damage = new Damage(
                _owner,
                null,
                _damageCount,
                TypesDamage.Clear);
            (_target as IHealthSystem).TakeDamage(damage);
        }
    }
}