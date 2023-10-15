namespace Assets.Scripts.Interfaces
{
    using Enums;
    using UnityEngine;

    public interface IAbility
    {
        public StatesOfAbility StateOfAbility { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="IAbility"/>
        /// </summary>
        public IAbility Construct(IMob owner, GameObject ownerGameObject);

        public void Cast();
    }
}