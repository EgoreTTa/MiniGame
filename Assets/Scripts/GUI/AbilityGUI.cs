namespace GUI
{
    using Abilities;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class AbilityGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _abilityImage;
        [SerializeField] private Image _abilityImageGUI;
        [SerializeField] private Image _abilityCooldown;
        [SerializeField] private Text _abilityCooldownTime;
        [SerializeField] private GameObject _hintPanel;
        [SerializeField] private Text _hintNameAbility;
        [SerializeField] private Text _hintTypingAbility;
        [SerializeField] private Text _hintSettingsAbility;
        [SerializeField] private Text _hintDescriptionAbility;
        [SerializeField] private BaseAbility _ability;

        public void UpdateAbilityReload(float amount, float time)
        {
            _abilityCooldown.fillAmount = amount;
            _abilityCooldownTime.text = time > 0 ? $"{time:F1}" : string.Empty;
        }

        public void SetAbility(BaseAbility ability)
        {
            _ability = ability;
            _abilityImageGUI.sprite = _ability.Sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Invoke(nameof(HintShow), 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HintHide();
        }

        private void HintShow()
        {
            if (_ability != null)
            {
                _hintNameAbility.text = _ability.NameAbility;
                _hintTypingAbility.text = _ability.TypingAbility;
                _hintSettingsAbility.text =
                    (_ability.Duration > 0 ? $" Длительность {_ability.Duration:F1}" : "") +
                    (_ability.Power > 0 ? $" Сила {_ability.Power:F1}" : "") +
                    (_ability.Radius > 0 ? $" Радиус {_ability.Radius:F1}" : "") +
                    (_ability.Range > 0 ? $" Радиус {_ability.Range:F1}" : "") +
                    (_ability.Cooldown > 0 ? $" Радиус {_ability.Cooldown:F1}" : "") +
                    (_ability.SpeedUse > 0 ? $" Радиус {_ability.SpeedUse:F1}" : "");
                _hintDescriptionAbility.text = _ability.DescriptionAbility;
                _hintPanel.SetActive(true);
            }
        }

        private void HintHide()
        {
            CancelInvoke(nameof(HintShow));
            _hintPanel.SetActive(false);
        }
    }
}