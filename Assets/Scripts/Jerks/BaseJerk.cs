namespace Jerks
{
    using Enums;
    using Mobs;
    using Movements;
    using UnityEngine;

    public abstract class BaseJerk : MonoBehaviour
    {
        public abstract StatesOfJerk StateOfJerk { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseJerk"/>
        /// </summary>
        public abstract BaseJerk Construct(
            BaseMob owner,
            Transform ownerTransform,
            Rigidbody2D ownerRigidbody2D,
            BaseMovement ownerMovementSystem);

        public abstract void Jerk(Vector3 direction);
    }
}