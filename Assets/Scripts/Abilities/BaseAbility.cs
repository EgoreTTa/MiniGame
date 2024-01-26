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
        [SerializeField] protected StatesOfAbility _stateOfAbility;
        [SerializeField] protected float _baseRadius;
        [SerializeField] protected float _baseSpeedUse;
        [SerializeField] protected float _baseCooldown;
        [SerializeField] protected float _basePower;
        [SerializeField] protected float _baseDuration;
        [SerializeField] protected float _baseRange;

        public string NameAbility => _nameAbility;
        public string TypingAbility => _typingAbility;
        public string DescriptionAbility => _descriptionAbility;
        public Sprite Sprite => _spriteAbility;
        public StatesOfAbility StateOfAbility => _stateOfAbility;
        public float BaseRadius => _baseRadius;
        public float BaseSpeedUse => _baseSpeedUse;
        public float BaseCooldown => _baseCooldown;
        public float BasePower => _basePower;
        public float BaseDuration => _baseDuration;
        public float BaseRange => _baseRange;
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