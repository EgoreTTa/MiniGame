namespace Assets.Scripts.Interfaces
{
    using UnityEngine;

    public interface IMoveSystem
    {
        public float MoveSpeed { get; set; }
        public float MinMoveSpeed { get; set; }
        public float MaxMoveSpeed { get; set; }
        public Vector3 Direction { get; }

        public void Move(Vector3 direction);
    }
}