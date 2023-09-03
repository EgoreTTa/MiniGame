namespace Assets.Scripts.Interfaces
{
    using Enemies;
    using NoMonoBehaviour;
    using UnityEngine;

    public interface IProjectile
    {
        public void Launch(float speed, Damage damage, Vector3 direction, float timeFly, BaseMob owner);
    }
}