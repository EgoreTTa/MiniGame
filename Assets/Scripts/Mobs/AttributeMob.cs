namespace Mobs
{
    using Abilities;
    using Attacks;
    using Jerks;
    using System.Collections.Generic;
    using Interfaces;
    using Movements;

    public class AttributeMob
    {
        private readonly BaseMob _ownerMob;
        private readonly List<IAttributeChange> _attributeChanges = new();

        private float _healthPointsMaximum = 100;
        private float _healthPointsMinimum = 0;
        private float _healthPointsRegeneration = 0;
        private readonly BaseHealthSystem _baseHealthSystem;

        private float _armorAbsoluteDamage = 100;
        private float _armorRelativeDamage = 0;
        private float _armorEffectResistance = 0;
        private BaseArmorSystem _baseArmorSystem;

        private float _attackAbsoluteDamage = 100;
        private float _attackRelativeDamage;
        private float _attackSpeed = 100;
        private BaseAttackSystem _baseAttackSystem;

        private float _abilityRadius = 100;
        private float _abilitySpeedUse = 100;
        private float _abilityCooldown = 100;
        private float _abilityPower = 100;
        private float _abilityDuration = 100;
        private float _abilityRange = 100;
        private readonly List<BaseAbility> _baseAbilities = new();

        private float _jerkDurationInvulnerability = 100;
        private float _jerkRange = 100;
        private float _jerkSpeed = 100;
        private BaseJerk _baseJerk;

        // private float _movementSpeed;
        private BaseMovement _baseMovement;

        public BaseAttackSystem AttackSystem
        {
            get => _baseAttackSystem;
            set
            {
                _baseAttackSystem = value;
                if (_baseAttackSystem != null)
                {
                    _baseAttackSystem.DamageCount = _attackAbsoluteDamage;
                    _baseAttackSystem.DamagePercent = _attackRelativeDamage;
                }

                NotifySubscribers();
            }
        }

        public BaseMovement Movement
        {
            get => _baseMovement;
            set => _baseMovement = value;
        }

        public BaseJerk BaseJerk
        {
            get => _baseJerk;
            set => _baseJerk = value;
        }

        public float HealthPointsMaximum
        {
            get => _healthPointsMaximum;
            set
            {
                _healthPointsMaximum = value;

                _baseHealthSystem.MaxHealth = _baseHealthSystem.BaseMaxHealth * _healthPointsMaximum / 100;
                NotifySubscribers();
            }
        }

        public float HealthPointsMinimum
        {
            get => _healthPointsMinimum;
            set
            {
                _healthPointsMinimum = value;

                _baseHealthSystem.MinHealth = _baseHealthSystem.BaseMinHealth * _healthPointsMinimum / 100;
                NotifySubscribers();
            }
        }

        public float HealthPointsRegeneration
        {
            get => _healthPointsRegeneration;
            set => _healthPointsRegeneration = value;
        }

        public float ArmorAbsoluteDamage
        {
            get => _armorAbsoluteDamage;
            set => _armorAbsoluteDamage = value;
        }

        public float ArmorRelativeDamage
        {
            get => _armorRelativeDamage;
            set => _armorRelativeDamage = value;
        }

        public float ArmorEffectResistance
        {
            get => _armorEffectResistance;
            set => _armorEffectResistance = value;
        }

        public float AttackAbsoluteDamage
        {
            get => _attackAbsoluteDamage;
            set
            {
                _attackAbsoluteDamage = value;
                if (_baseAttackSystem != null) _baseAttackSystem.DamageCount = value;
                NotifySubscribers();
            }
        }

        public float AttackRelativeDamage
        {
            get => _attackRelativeDamage;
            set
            {
                _attackRelativeDamage = value;
                if (_baseAttackSystem != null) _baseAttackSystem.DamagePercent = value;
                NotifySubscribers();
            }
        }

        public float AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }

        public float AbilityRadius
        {
            get => _abilityRadius;
            set
            {
                _abilityRadius = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.Radius = ability.BaseRadius * _abilityRadius / 100;
                }
                NotifySubscribers();
            }
        }

        public float AbilitySpeedUse
        {
            get => _abilitySpeedUse;
            set
            {
                _abilitySpeedUse = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.SpeedUse = ability.SpeedUse * _abilitySpeedUse / 100;
                }
                NotifySubscribers();
            }
        }

        public float AbilityCooldown
        {
            get => _abilityCooldown;
            set
            {
                _abilityCooldown = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.Cooldown = ability.BaseCooldown * _abilityCooldown / 100;
                }
                NotifySubscribers();
            }
        }

        public float AbilityPower
        {
            get => _abilityPower;
            set
            {
                _abilityPower = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.Power = ability.BasePower * _abilityPower / 100;
                }
                NotifySubscribers();
            }
        }

        public float AbilityDuration
        {
            get => _abilityDuration;
            set
            {
                _abilityDuration = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.Duration = ability.BaseDuration * _abilityDuration / 100;
                }
                NotifySubscribers();
            }
        }

        public float AbilityRange
        {
            get => _abilityRange;
            set
            {
                _abilityRange = value;
                foreach (var ability in _baseAbilities)
                {
                    ability.Range = ability.BaseRange * _abilityRange / 100;
                }
                NotifySubscribers();
            }
        }

        public float JerkDurationInvulnerability
        {
            get => _jerkDurationInvulnerability;
            set => _jerkDurationInvulnerability = value;
        }

        public float JerkRange
        {
            get => _jerkRange;
            set => _jerkRange = value;
        }

        public float JerkSpeed
        {
            get => _jerkSpeed;
            set => _jerkSpeed = value;
        }

        public BaseArmorSystem ArmorSystem
        {
            get => _baseArmorSystem;
            set => _baseArmorSystem = value;
        }

        public BaseJerk Jerk
        {
            get => _baseJerk;
            set => _baseJerk = value;
        }

        public BaseAbility[] BaseAbilities => _baseAbilities.ToArray();

        public AttributeMob(BaseMob ownerMob)
        {
            _ownerMob = ownerMob;
            _baseHealthSystem = ownerMob.HealthSystem;
        }

        public void AddAbility(BaseAbility baseAbility)
        {
            _baseAbilities.Add(baseAbility);
        }

        public void RemoveAbility(BaseAbility baseAbility)
        {
            _baseAbilities.Remove(baseAbility);
        }

        public void Subscribe(IAttributeChange attributeChange)
        {
            _attributeChanges.Add(attributeChange);
        }

        public void Unsubscribe(IAttributeChange attributeChange)
        {
            _attributeChanges.Remove(attributeChange);
        }

        private void NotifySubscribers()
        {
            foreach (var attributeChange in _attributeChanges)
            {
                attributeChange.AttributeChange(this);
            }
        }
    }
}