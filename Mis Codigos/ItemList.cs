using System.Collections.Generic;

[System.Serializable]
public class ItemList
{
    public string listName;
    public List<Item> items;

    public ItemList(string name)
    {
        listName = name;
        items = new List<Item>();
    }
}
