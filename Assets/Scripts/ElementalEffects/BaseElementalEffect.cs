namespace Assets.Scripts.ElementalEffects
{
    using System;
    using System.Linq;
    using Interfaces;
    using UnityEngine;

    [RequireComponent(typeof(IMob))]
    public abstract class BaseElementalEffect : MonoBehaviour
    {
        [Flags]
        public enum TypesElement
        {
            ElementalEffect1 = 1,
            ElementalEffect2 = 2,
            Effect3 = 4,
            Effect4 = 8,
            Effect5 = 16,

            ElementalEffect12 = ElementalEffect1 | ElementalEffect2,
            Effect13 = ElementalEffect1 | Effect3,
            Effect14 = ElementalEffect1 | Effect4,
            Effect15 = ElementalEffect1 | Effect5,
            Effect23 = ElementalEffect2 | Effect3,
            Effect24 = ElementalEffect2 | Effect4,
            Effect25 = ElementalEffect2 | Effect5,
            Effect34 = Effect3 | Effect4,
            Effect35 = Effect3 | Effect5,
            Effect45 = Effect4 | Effect5,
        }

        protected IMob _target;
        protected IMob _owner;
        [SerializeField] protected TypesElement _typeElement;

        public TypesElement TypeElement => _typeElement;

        public IMob Target => _target;

        public IMob Owner => _owner;

        private void Awake()
        {
            _target = GetComponent<IMob>();
        }

        protected void CombineEffect()
        {
            var elementalEffects = GetComponents<BaseElementalEffect>();
            elementalEffects = elementalEffects.Where(x =>
                    x.TypeElement is TypesElement.ElementalEffect1 or
                        TypesElement.ElementalEffect2 or
                        TypesElement.Effect3 or
                        TypesElement.Effect4 or
                        TypesElement.Effect5)
                .ToArray();
            if (elementalEffects.Length > 1)
            {
                var elementalEffect1 = elementalEffects[0];
                var elementalEffect2 = elementalEffects[1];

                switch (elementalEffect1.TypeElement |
                        elementalEffect2.TypeElement)
                {
                    case TypesElement.ElementalEffect12:
                        Destroy(elementalEffect1);
                        Destroy(elementalEffect2);
                        gameObject.AddComponent<ElementalEffect12>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}