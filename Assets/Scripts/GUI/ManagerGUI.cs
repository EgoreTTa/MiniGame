namespace Assets.Scripts.GUI
{
    using UnityEngine;

    public class ManagerGUI : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;

        public void UpdateHealthBar(float health, float maxHealth)
        {
            _healthBar.UpdateBar(health, maxHealth);
        }
    }
}