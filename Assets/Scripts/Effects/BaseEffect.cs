namespace Assets.Scripts.Effects
{
    using UnityEngine;

    public abstract class BaseEffect : MonoBehaviour
    {
        public abstract string EffectName { get; }
        public abstract bool IsActive { get; set; }
    }
}