using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public InputField listNameInputField;
    public GameObject itemListPrefab;
    public Transform itemListContainer;
    public InputField itemNameInputField;
    public InputField itemDescriptionInputField;
    public GameObject itemCreationPanel;
    public GameObject itemListDisplayPanel; // Panel to display items in a list
    public Transform itemDisplayContainer; // Container for item entries inside a list
    public GameObject itemEntryPrefab; // Prefab for each item entry
    public Text itemListTitleText; // Title text element for the list name

    private List<ItemList> itemLists = new List<ItemList>();
    private ItemList currentItemList;

    public void CreateNewItemList()
    {
        string listName = listNameInputField.text;

        if (!string.IsNullOrEmpty(listName))
        {
            // Create a new ItemList and set it as the current item list
            currentItemList = new ItemList(listName);
            itemLists.Add(currentItemList);

            Debug.Log("New item list created: " + listName);

            listNameInputField.text = ""; // Clear input field
            AddItemListToUI(currentItemList); // Update UI
            itemCreationPanel.SetActive(true); // Show item creation panel
        }
        else
        {
            Debug.LogWarning("List name cannot be empty!");
        }
    }

    private void AddItemListToUI(ItemList itemList)
    {
        GameObject itemEntry = Instantiate(itemListPrefab, itemListContainer);

        // Set the text component to display the list name
        Text itemText = itemEntry.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = itemList.listName;
        }

        // Set up the "View Items" button dynamically
        Button viewItemsButton = itemEntry.GetComponentInChildren<Button>();
        if (viewItemsButton != null)
        {
            viewItemsButton.onClick.AddListener(() => ViewItemList(itemList));
        }
    }

    public void AddItemToCurrentList()
    {
        string itemName = itemNameInputField.text;
        string itemDescription = itemDescriptionInputField.text;

        if (currentItemList != null && !string.IsNullOrEmpty(itemName))
        {
            Item newItem = new Item(itemName, itemDescription);
            currentItemList.items.Add(newItem);

            Debug.Log("Item added: " + itemName + " - " + itemDescription);

            itemNameInputField.text = ""; // Clear input fields
            itemDescriptionInputField.text = "";
        }
        else
        {
            Debug.LogWarning("Item name cannot be empty or no list is selected.");
        }
    }

    public void FinalizeCurrentList()
    {
        if (currentItemList != null)
        {
            Debug.Log("Finalized list: " + currentItemList.listName + " with " + currentItemList.items.Count + " items.");

            itemCreationPanel.SetActive(false); // Hide item creation panel
            currentItemList = null; // Reset for new list
        }
        else
        {
            Debug.LogWarning("No list to finalize.");
        }
    }

    public void ViewItemList(ItemList itemList)
    {
        itemListDisplayPanel.SetActive(true); // Show the item display panel

        // Set the title to the name of the list
        itemListTitleText.text = itemList.listName;

        // Clear any previous item entries from the container
        foreach (Transform child in itemDisplayContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate an item entry for each item in the list
        foreach (Item item in itemList.items)
        {
            GameObject itemEntry = Instantiate(itemEntryPrefab, itemDisplayContainer);
            Text itemText = itemEntry.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = item.itemName; // Display only the item name
            }
        }
    }
}
