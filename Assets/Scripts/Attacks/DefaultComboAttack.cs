namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using System;
    using UnityEngine;

    public class DefaultComboAttack : MonoBehaviour
    {
        private IMob _owner;

        private void Awake()
        {
            if (GetComponentInParent<IMob>() is { } mob) _owner = mob;
            else throw new Exception($"{nameof(DefaultComboAttack)} not instance {nameof(IMob)}");
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<IHealthSystem>() is { } healthSystem)
            {
                if (collider.gameObject != (_owner as MonoBehaviour)!.gameObject)
                {
                    var damageCount = (_owner as MonoBehaviour)!.GetComponentInChildren<IAttackSystem>().DamageCount;
                    var damage = new Damage(_owner, null, damageCount, TypesDamage.Clear);
                    healthSystem.TakeDamage(damage);
                }
            }
        }
    }
}