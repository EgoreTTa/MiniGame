namespace Assets.Scripts.Effects
{
    using Interfaces;
    using UnityEngine;

    public abstract class BaseEffect : MonoBehaviour
    {
        [SerializeField] protected string _effectName;

        public string EffectName => _effectName;

        public abstract void StartEffect(IMob target);
    }
}