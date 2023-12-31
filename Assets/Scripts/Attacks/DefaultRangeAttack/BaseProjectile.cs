namespace Assets.Scripts.Attacks.DefaultRangeAttack
{
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    public abstract class BaseProjectile : MonoBehaviour
    {
        public abstract void Launch(
            float speed,
            Damage damage,
            Vector3 direction,
            float timeFly,
            BaseMob owner);
    }
}