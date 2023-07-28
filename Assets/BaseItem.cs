using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] protected string _nameItem;

    public abstract void Use(BaseMob target);
}