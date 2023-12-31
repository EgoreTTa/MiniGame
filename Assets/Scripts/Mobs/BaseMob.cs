namespace Assets.Scripts.Mobs
{
    using Interfaces;
    using Attacks;
    using Enums;
    using Movements;
    using UnityEngine;

    public abstract class BaseMob : MonoBehaviour
    {
        public abstract string FirstName { get; }
        public abstract GroupsMobs GroupMobs { get; }
        public abstract BaseHealthSystem HealthSystem { get; }
        public abstract BaseMovement MovementSystem { get; }
        public abstract BaseAttackSystem AttackSystem { get; }

        public abstract void KilledMob(BaseMob mob);
        public abstract void Subscribe(IKillerMob killerMob);
        public abstract void Unsubscribe(IKillerMob killerMob);
    }
}