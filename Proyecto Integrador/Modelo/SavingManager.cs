using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SavingManager : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        // Define the file path using persistent data path (works for all platforms)
        filePath = Path.Combine(Application.persistentDataPath, "savedItems.json");
        Debug.Log("File path: " + filePath);
    }

    // Save both item lists to a file
    public void SaveItemLists(List<ItemList> firstItemList, List<ItemList> secondItemList)
    {
        // Create a wrapper for both lists
        ItemListsWrapper wrapper = new ItemListsWrapper(firstItemList, secondItemList);

        // Serialize the wrapper to JSON
        string json = JsonUtility.ToJson(wrapper);

        // Write the serialized JSON to the file
        File.WriteAllText(filePath, json);
        Debug.Log("Both item lists saved.");
    }

    // Load item lists from a file
    // Load only the first item list from the file
    public List<ItemList> LoadFirstItemList()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON into the wrapper
            ItemListsWrapper wrapper = JsonUtility.FromJson<ItemListsWrapper>(json);

            // Return the first list (if it exists, otherwise empty)
            return wrapper.firstItemList ?? new List<ItemList>();
        }
        return new List<ItemList>();  // Return an empty list if no file found
    }

    // Load only the second item list from the file
    public List<ItemList> LoadSecondItemList()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON into the wrapper
            ItemListsWrapper wrapper = JsonUtility.FromJson<ItemListsWrapper>(json);

            // Return the second list (if it exists, otherwise empty)
            return wrapper.secondItemList ?? new List<ItemList>();
        }
        return new List<ItemList>();  // Return an empty list if no file found
    }


    // Wrapper class to hold both lists (first and second)
    [System.Serializable]
    public class ItemListsWrapper
    {
        public List<ItemList> firstItemList;
        public List<ItemList> secondItemList;

        // Constructor to initialize both lists
        public ItemListsWrapper(List<ItemList> firstItemList, List<ItemList> secondItemList)
        {
            this.firstItemList = firstItemList;
            this.secondItemList = secondItemList;
        }
    }


}
