namespace Assets.Scripts.NoMonoBehaviour
{
    using Interfaces;
    using UnityEngine;

    public class Health
    {
        private IMob _owner;
        private GameObject _healthBy;
        private float _countHealth;

        public IMob Owner => _owner;
        public GameObject HealthBy => _healthBy;
        public float CountHealth => _countHealth;

        public Health(
            IMob owner,
            GameObject healthBy,
            float countHealth)
        {
            _owner = owner;
            _healthBy = healthBy;
            _countHealth = countHealth;
        }
    }
}