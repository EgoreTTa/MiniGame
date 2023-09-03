namespace Assets.Scripts.ElementalEffects
{
    using Interfaces;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class ElementalEffect2 : BaseElementalEffect
    {
        [SerializeField] private float _changeMoveSpeed = 1f;
        [SerializeField] private float _timeOfAction = 10f;

        private void Start()
        {
            _typeElement = TypesElement.ElementalEffect2;
            CombineEffect();
            if (_target.GetComponent<IMoveSystem>() is { } moveSystem)
            {
                moveSystem.MoveSpeed -= _changeMoveSpeed;
                Destroy(this, _timeOfAction);
            }
            else
                Destroy(this);
        }

        private void OnDestroy()
        {
            if (_target.GetComponent<IMoveSystem>() is { } moveSystem)
                moveSystem.MoveSpeed += _changeMoveSpeed;
        }
    }
}