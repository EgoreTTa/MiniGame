namespace Assets.Scripts.Movements
{
    using UnityEngine;

    public abstract class BaseMovement : MonoBehaviour
    {
        public abstract float MoveSpeed { get; set; }
        public abstract float MinMoveSpeed { get; set; }
        public abstract float MaxMoveSpeed { get; set; }
        public abstract Vector3 Direction { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseMovement"/>
        /// </summary>
        public abstract BaseMovement Construct(Transform transform);

        public abstract void Move(Vector3 direction);
    }
}