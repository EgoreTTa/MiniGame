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

        public string NameAbility => _nameAbility;
        public string TypingAbility => _typingAbility;
        public string DescriptionAbility => _descriptionAbility;
        public Sprite Sprite => _spriteAbility;
        public abstract StatesOfAbility StateOfAbility { get; }
        public abstract float Radius { get; set; }
        public abstract float SpeedUse { get; set; }
        public abstract float Cooldown { get; set; }
        public abstract float Power { get; set; }
        public abstract float Duration { get; set; }
        public abstract float Range { get; set; }

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