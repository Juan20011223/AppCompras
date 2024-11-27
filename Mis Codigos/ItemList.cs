using System.Collections.Generic;
using System;

[Serializable]
public class ItemList
{
    public string listName;
    public List<Item> items;

    // Constructor to initialize the item list
    public ItemList(string listName)
    {
        this.listName = listName;
        this.items = new List<Item>();
    }
}
