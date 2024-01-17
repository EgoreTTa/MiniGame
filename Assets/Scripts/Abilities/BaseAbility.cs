namespace Abilities
{
    using Enums;
    using GUI;
    using Mobs;
    using UnityEngine;

    public abstract class BaseAbility : MonoBehaviour
    {
        [SerializeField] protected string _nameAbility;
        [SerializeField] protected string _typingAbility;
        [SerializeField] protected string _descriptionAbility;
        [SerializeField] protected Sprite _spriteAbility;
        [SerializeField] protected ManagerGUI _managerGUI;

        public abstract StatesOfAbility StateOfAbility { get; }

        /// <summary>
        /// Constructor for <see cref="MonoBehaviour"/> is <see cref="BaseAbility"/>
        /// </summary>
        public abstract BaseAbility Construct(
            BaseMob owner,
            GroupsMobs ownerGroupMobs,
            GameObject ownerGameObject,
            ManagerGUI managerGUI);

        public abstract void Cast();
    }
}