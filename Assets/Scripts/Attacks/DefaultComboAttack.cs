namespace Assets.Scripts.Attacks
{
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using System;
    using UnityEngine;

    public class DefaultComboAttack : MonoBehaviour
    {
        private BaseMob _owner;

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception($"{nameof(DefaultComboAttack)} not instance {nameof(BaseMob)}");
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