using System.Collections.Generic;

public class Inventory
{
    private List<BaseItem> _items = new();

    public BaseItem[] Items => _items.ToArray();

    public void Put(BaseItem item)
    {
        _items.Add(item);
    }

    public void Take(BaseItem item)
    {
        _items.Remove(item);
    }
}