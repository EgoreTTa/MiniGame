namespace Assets.Scripts.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _health;
        [SerializeField] private Text _text;
        private Gradient gradient = new ();

        private void Awake()
        {
            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(Color.red, 0.0f);
            colors[1] = new GradientColorKey(Color.green, 1.0f);
            var alphas = new GradientAlphaKey[2];
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(1.0f, 1.0f);
            gradient.SetKeys(colors, alphas);
        }

        public void UpdateBar(float health, float maxHealth)
        {
            var percent = health / maxHealth;
            _health.fillAmount = percent;
            _health.color = gradient.Evaluate(percent);
            _text.text = $"{health}/{maxHealth}";
        }
    }
}