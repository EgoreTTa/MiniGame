namespace Assets.Scripts.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _health;
        [SerializeField] private Text _text;

        public void UpdateBar(float health, float maxHealth)
        {
            var percent = health / maxHealth;
            _health.fillAmount = percent;
            _background.color = percent < .5f ? Color.red : Color.green;
            _text.text = $"{health}/{maxHealth}";
        }
    }
}