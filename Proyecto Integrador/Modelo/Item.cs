[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public float price;
    public string imagePath; // Path to the image

    // Constructor to initialize the item
    public Item(string itemName, string itemDescription, float price, string imagePath)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.price = price;
        this.imagePath = imagePath;
    }
}
