namespace Assets.Scripts.Interfaces
{
    using Enums;
    using UnityEngine;

    public interface IJerk
    {
        public StatesOfJerk StateOfJerk { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IJerk"/>
        /// </summary>
        public IJerk Construct(
            IMob owner,
            Transform ownerTransform,
            Rigidbody2D ownerRigidbody2D,
            IMovementSystem ownerMovementSystem);

        public void Jerk();
    }
}