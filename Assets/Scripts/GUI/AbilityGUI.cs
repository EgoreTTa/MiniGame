namespace GUI
{
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
        [SerializeField] private Text _hintDescriptionAbility;

        public void UpdateAbilityReload(float amount, float time)
        {
            _abilityCooldown.fillAmount = amount;
            _abilityCooldownTime.text = time > 0 ? $"{time:F1}" : string.Empty;
        }

        public void SetAbility(Sprite spriteAbility, string nameAbility, string hintTypingAbility, string descriptionAbility)
        {
            _abilityImageGUI.sprite = spriteAbility;
            _hintNameAbility.text = nameAbility;
            _hintTypingAbility.text = hintTypingAbility;
            _hintDescriptionAbility.text = descriptionAbility;
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
            _hintPanel.SetActive(true);
        }

        private void HintHide()
        {
            CancelInvoke(nameof(HintShow));
            _hintPanel.SetActive(false);
        }
    }
}