using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField] protected string _nameItem;
    [SerializeField] protected string _description;
    [SerializeField] protected int _scoreCount;
    [SerializeField] protected Sprite _spriteItem;

    public string NameItem => _nameItem;
    public string Description => _description;
    public int ScoreCount => _scoreCount;
    public Sprite SpriteItem => _spriteItem;

    public abstract void PickUp(BaseMob parent);
    public virtual void PickDown(BaseMob parent) { }
}