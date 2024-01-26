namespace GUI
{
    using Mobs;
    using UnityEngine;
    using UnityEngine.UI;

    public class AttributeGUI : MonoBehaviour
    {
        [SerializeField] private Text _attackAbsoluteDamage;
        [SerializeField] private Text _attackRelativeDamage;
        [SerializeField] private Text _attackSpeed;

        [SerializeField] private Text _healthPointsMaximum;
        [SerializeField] private Text _healthPointsMinimum;
        [SerializeField] private Text _healthPointsRegeneration;

        [SerializeField] private Text _abilityRadius;
        [SerializeField] private Text _abilitySpeedUse;
        [SerializeField] private Text _abilityCooldown;
        [SerializeField] private Text _abilityPower;
        [SerializeField] private Text _abilityDuration;
        [SerializeField] private Text _abilityRange;

        public void AttributeChange(AttributeMob attributeMob)
        {
            _attackAbsoluteDamage.text = $"{attributeMob.AttackAbsoluteDamage}";
            _attackRelativeDamage.text = $"{attributeMob.AttackRelativeDamage}%";
            _attackSpeed.text = $"{attributeMob.AttackSpeed}";
            _healthPointsMaximum.text = $"{attributeMob.HealthPointsMaximum}%";
            _healthPointsMinimum.text = $"{attributeMob.HealthPointsMinimum}%";
            _healthPointsRegeneration.text = $"{attributeMob.HealthPointsRegeneration}";
            _abilityRadius.text = $"{attributeMob.AbilityRadius}%";
            _abilitySpeedUse.text = $"{attributeMob.AbilitySpeedUse}%";
            _abilityCooldown.text = $"{attributeMob.AbilityCooldown}%";
            _abilityPower.text = $"{attributeMob.AbilityPower}%";
            _abilityDuration.text = $"{attributeMob.AbilityDuration}%";
            _abilityRange.text = $"{attributeMob.AbilityRange}%";
        }
    }
}