namespace Assets.Scripts.Interfaces
{
    using UnityEngine;

    public interface IMovementSystem
    {
        public float MoveSpeed { get; set; }
        public float MinMoveSpeed { get; set; }
        public float MaxMoveSpeed { get; set; }
        public Vector3 Direction { get; }


        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IMovementSystem"/>
        /// </summary>
        public IMovementSystem Construct(Transform transform);

        public void Move(Vector3 direction);
    }
}