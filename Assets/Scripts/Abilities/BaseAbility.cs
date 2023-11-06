namespace Assets.Scripts.Abilities
{
    using Enums;
    using Mobs;
    using UnityEngine;

    public abstract class BaseAbility : MonoBehaviour
    {
        public abstract StatesOfAbility StateOfAbility { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseAbility"/>
        /// </summary>
        public abstract BaseAbility Construct(BaseMob owner, GroupsMobs ownerGroupMobs, GameObject ownerGameObject);

        public abstract void Cast(Vector3? position = null, Vector3? direction = null);
    }
}