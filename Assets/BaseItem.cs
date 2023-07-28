using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] protected string _itemName;

    public virtual void PreUse(BaseMob parent) { }
    public abstract void Use(BaseMob parent);
    public virtual void EndUse(BaseMob parent) { }
}