namespace Assets.Scripts.ElementalEffects
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public class ElementalEffect2 : BaseElementalEffect
    {
        [SerializeField] private float _changeMoveSpeed = 1f;
        [SerializeField] private float _timeOfAction = 10f;
        private float _timerOfAction;

        private void Start()
        {
            _typeElement = TypesElement.ElementalEffect2;
            CombineEffect();
            _target.MoveSpeed -= _changeMoveSpeed;
        }

        private void Update()
        {
            if (_timerOfAction < _timeOfAction)
            {
                _timerOfAction += Time.deltaTime;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            _target.MoveSpeed += _changeMoveSpeed;
        }
    }
}