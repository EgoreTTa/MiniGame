namespace Assets.Scripts.Abilities
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class InvokeGhostCA : MonoBehaviour, IAbility
    {
        [SerializeField] private StatesOfAbility _stateOfAbility;
        [SerializeField] private float _timeToSwing;
        [SerializeField] private float _timeToCasted;
        [SerializeField] private float _timeToRecovery;
        [SerializeField] private float _timeReload;
        [SerializeField] private bool _isReady;
        [SerializeField] private GameObject _ghostCA;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;
        private GameObject MyGhostCA;
        
        public StatesOfAbility StateOfAbility => _stateOfAbility;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Ready()
        {
            _isReady = true;
        }

        private void IntoCasted()
        {
            _stateOfAbility = StatesOfAbility.Casted;
            MyGhostCA = Instantiate(_ghostCA, transform);
            MyGhostCA.gameObject.SetActive(true);
            MyGhostCA.transform.parent = null;
            Invoke(nameof(IntoRecovery), _timeToCasted);
        }

        private void IntoRecovery()
        {
            _stateOfAbility = StatesOfAbility.Recovery;
            Invoke(nameof(IntoStandby), _timeToRecovery);
        }

        private void IntoStandby()
        {
            _stateOfAbility = StatesOfAbility.Standby;
        }

        public void Cast()
        {
            if (_stateOfAbility != StatesOfAbility.Standby
                ||
                _isReady is false) return;

            _isReady = false;
            Invoke(nameof(Ready), _timeReload);
            Swing();
        }

        private void Swing()
        {
            _stateOfAbility = StatesOfAbility.Swing;
            Invoke(nameof(IntoCasted), _timeToSwing);
        }
    }
}
