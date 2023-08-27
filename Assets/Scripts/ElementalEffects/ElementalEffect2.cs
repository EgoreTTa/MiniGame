namespace Assets.Scripts.ElementalEffects
{
    using Interfaces;
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
            if (_target is IMoveSystem moveSystem)
                moveSystem.MoveSpeed -= _changeMoveSpeed;
            else
                Destroy(this);
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
            if (_target is IMoveSystem moveSystem)
                moveSystem.MoveSpeed += _changeMoveSpeed;
        }
    }
}