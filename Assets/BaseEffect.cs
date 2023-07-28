using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] protected string _effectName;

    public string EffectName => _effectName;

    public virtual void PreStartEffect(BaseMob target) { }
    public abstract void StartEffect(BaseMob target);
    public virtual void EndEffect(BaseMob target) { }
}