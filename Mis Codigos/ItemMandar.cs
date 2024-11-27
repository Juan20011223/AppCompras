[System.Serializable]
public class ItemMandar
{
    public string itemName;
    public float price;

    // Constructor to initialize the item
    public ItemMandar(string itemName, float price)
    {
        this.itemName = itemName;
        this.price = price;
    }
}
