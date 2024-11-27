using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public InputField listNameInputField;
    public GameObject itemListPrefab;
    public Transform itemListContainer;
    public InputField itemNameInputField;
    public InputField itemDescriptionInputField;
    public InputField itemPriceInputField;
    public GameObject itemCreationPanel;
    public GameObject itemListDisplayPanel; // Panel to display items in a list
    public Transform itemDisplayContainer; // Container for item entries inside a list
    public GameObject itemEntryPrefab; // Prefab for each item entry
    public Text itemListTitleText; // Title text element for the list name

    public GameObject itemEditListPrefab;
    public Transform itemEditListContainer;
    public GameObject itemEditListDisplayPanel; // Panel to display items in a list
    public Transform itemEditDisplayContainer; // Container for item entries inside a list
    public GameObject itemEditEntryPrefab; // Prefab for each item entry
    public GameObject panelEditItem;

    public GameObject itemScanListPrefab;
    public Transform itemScanListContainer;
    public GameObject itemScanListDisplayPanel;
    public GameObject panelScanner;

    public GameObject itemShareableListPrefab;
    public Transform itemShareableListContainer;


    public GameObject itemShareablePrefab;
    public Transform itemShareableContainer;
    public GameObject itemListSharableDisplayPanel; // Panel to display items in a list
    public Text itemShareableListTitleText; // Title text element for the list name
    public Text totalPrecioTxt; // Title text element for the list name


    public InputField itemNameDefault;
    public InputField itemDescriptionDefault;
    public InputField itemPriceDefault;

    public List<ItemList> itemLists = new List<ItemList>();
    public ItemList currentItemList;

    public List<ItemList> itemListsShareable = new List<ItemList>();
    public ItemList currentItemListShareable;

    public ItemList temporaryList;

    public SavingManager savingManager;

    public GameObject scannerCamera;

    public CameraManager cameraManager;

    // Add reference to the ImageTargetManager
    public ImageTargetManager imageTargetManager;

    public GameObject panelAviso;

    public CanvasManager canvasManager;

    void Start()
    {

        // Load saved item lists when the app starts
        itemLists = savingManager.LoadFirstItemList();
        // Load saved item lists when the app starts
        itemListsShareable = savingManager.LoadSecondItemList();
        foreach (var list in itemLists)
        {
            AddItemListToUI(list); // Update UI with saved item lists
            AddItemEditListToUI(list); // Update edit UI
            AddItemScanListToUI(list); // Update edit UI
        }

        foreach (var list in itemListsShareable)
        {
            AddItemListToShareableUI(list); // Update UI with saved item lists
        }
    }

    public void CreateNewShareableItemList(string name, string baseName)
    {
        string listName = name;
        temporaryList = itemLists.Find(list => list.listName == baseName);

        if (temporaryList != null)
        {
            // Make sure to pass the listName to the ItemList constructor
            temporaryList = new ItemList(temporaryList.listName)
            {
                items = new List<Item>(temporaryList.items.Select(item => new Item(
                    item.itemName,           // Pass itemName from the original item
                    item.itemDescription,    // Pass itemDescription
                    item.price,              // Pass price
                    item.imagePath           // Pass imagePath
                )))
            };
        }

        if (!string.IsNullOrEmpty(listName))
        {
            currentItemListShareable = new ItemList(listName);
            //itemListsShareable.Add(currentItemListShareable);

            Debug.Log("New item list created: " + listName);

            listNameInputField.text = ""; // Clear input field
        }
        else
        {
            Debug.LogWarning("List name cannot be empty!");
        }
    }


    private void AddItemListToShareableUI(ItemList itemList)
    {
        GameObject itemEntry = Instantiate(itemShareableListPrefab, itemShareableListContainer);
        Text itemText = itemEntry.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = itemList.listName;
        }

        Button viewItemsButton = itemEntry.GetComponentInChildren<Button>();
        if (viewItemsButton != null)
        {
            viewItemsButton.onClick.AddListener(() => ViewItemListShareable(itemList));
       }
    }

    public void ViewItemListShareable(ItemList itemList)
    {
        itemListSharableDisplayPanel.SetActive(true);
        itemShareableListTitleText.text = itemList.listName;

        // Clear existing children in the container
        foreach (Transform child in itemShareableContainer)
        {
            Destroy(child.gameObject);
        }

        float totalPrice = 0f; // Initialize the total price variable

        // Populate the container with items
        foreach (Item item in itemList.items)
        {
            GameObject itemEntry = Instantiate(itemShareablePrefab, itemShareableContainer);

            // Get all Text components in the prefab
            Text[] textComponents = itemEntry.GetComponentsInChildren<Text>();
            if (textComponents != null && textComponents.Length >= 2)
            {
                // Assign the item name to the first Text component
                textComponents[0].text = item.itemName;

                // Assign the price to the second Text component in the desired format
                textComponents[1].text = $"Precio: {item.price}";

                // Add the item's price to the total price
                totalPrice += item.price;
            }
            else
            {
                Debug.LogError("The prefab does not contain enough Text components for item details.");
            }
        }

        // Update the totalPrecioTxt with the total price in the desired format
        if (totalPrecioTxt != null)
        {
            totalPrecioTxt.text = $"Total Precio: {totalPrice}";
        }
        else
        {
            Debug.LogError("totalPrecioTxt is not assigned in the inspector.");
        }
    }

    


    public bool CompareList(string listName)
    {
        // Loop through all the item lists in itemListsShareable
        foreach (var itemList in itemListsShareable)
        {
            // Compare the name of the current item list with the provided name
            if (itemList.listName == listName)
            {
                Debug.Log("aaaaaaaaaaaaa  "+itemList.listName + " " + listName);
                // Return true if a match is found
                return true;
            }
        }

        // Return false if no match is found
        return false;
    }


    public void AddItemToCurrentList(Item newItem)
    {
 
        currentItemListShareable.items.Add(newItem);

    }

    public void DiscardItems()
    {
        if (temporaryList != null && currentItemListShareable != null)
        {

            Debug.Log("Contents of currentItemListShareableaaaaaa:");
            foreach (var item in currentItemListShareable.items)
            {
                Debug.Log($"Item: {item.itemName}"); // Prints the item name
            }

            for (int i = temporaryList.items.Count - 1; i >= 0; i--) // Iterate backward to safely remove items
            {
                if (currentItemListShareable.items.Exists(item => item.itemName == temporaryList.items[i].itemName))
                {
                    temporaryList.items.RemoveAt(i);

                    foreach (var item in temporaryList.items)
                    {
                        Debug.Log($"Item: {item.itemName}"); // Prints the item name
                    }
                }
            }

            Debug.Log("Removed matching items from temporaryList.");

            // Set currentItemListShareable to temporaryList
            currentItemListShareable = temporaryList;
            itemListsShareable.Add(currentItemListShareable);
            Debug.Log("currentItemListShareable is now set to temporaryList.");

            // Print the contents of currentItemListShareable
            Debug.Log("Contents of currentItemListShareable:");
            foreach (var item in currentItemListShareable.items)
            {
                Debug.Log($"Item: {item.itemName}"); // Prints the item name
            }

            // Print the contents of temporaryList
            Debug.Log("Contents of temporaryList:");
            foreach (var item in temporaryList.items)
            {
                Debug.Log($"Item: {item.itemName}"); // Prints the item name
            }
        }
        else
        {
            Debug.LogWarning("Either temporaryList or currentItemListShareable is null.");
        }

        savingManager.SaveItemLists(itemLists, itemListsShareable);
        AddItemListToShareableUI(currentItemListShareable); // Update UI
    }




    public void CreateNewItemList()
    {
        string listName = listNameInputField.text;

        if (!string.IsNullOrEmpty(listName))
        {
            currentItemList = new ItemList(listName);
            itemLists.Add(currentItemList);

            Debug.Log("New item list created: " + listName);

            listNameInputField.text = ""; // Clear input field
            AddItemListToUI(currentItemList); // Update UI
            AddItemEditListToUI(currentItemList); // Update UI
            AddItemScanListToUI(currentItemList); // Update UI
            itemCreationPanel.SetActive(true); // Show item creation panel

            // Save the item lists after creation
            savingManager.SaveItemLists(itemLists, itemListsShareable);
        }
        else
        {
            canvasManager.SetPuedeSeguir(false);
            panelAviso.SetActive(true);
            Debug.LogWarning("List name cannot be empty!");
        }
    }

    private void AddItemListToUI(ItemList itemList)
    {
        GameObject itemEntry = Instantiate(itemListPrefab, itemListContainer);
        Text itemText = itemEntry.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = itemList.listName;
        }

        Button viewItemsButton = itemEntry.GetComponentInChildren<Button>();
        if (viewItemsButton != null)
        {
            viewItemsButton.onClick.AddListener(() => ViewItemList(itemList));
        }
    }

    private void AddItemScanListToUI(ItemList itemList)
    {
        // Instantiate the item entry prefab
        GameObject itemEntry = Instantiate(itemScanListPrefab, itemScanListContainer);

        // Set the item text (item list name)
        Text itemText = itemEntry.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = itemList.listName;
        }

        // Find the button and add a listener to it
        Button viewItemsButton = itemEntry.GetComponentInChildren<Button>();
        if (viewItemsButton != null)
        {
            // This is where we set the currentItemList before opening the scanner
            viewItemsButton.onClick.AddListener(() =>
            {
                // Set the current item list when the button is clicked
                currentItemList = itemList;

                // Open the scanner with the selected item list
                OpenScanner();
            });
        }
    }


    private void OpenScanner()
    {
        // Make sure we have the currentItemList selected
        if (currentItemList != null)
        {
            // Pass the itemList to the ImageTargetManager
            imageTargetManager.SetItemList(currentItemList); // Assuming you have a SetItemList method in ImageTargetManager

            // Activate the camera and panels as needed
            scannerCamera.SetActive(true);
            itemScanListDisplayPanel.SetActive(false);
            panelScanner.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No item list selected for scanning!");
        }
    }


    private void AddItemEditListToUI(ItemList itemList)
    {
        GameObject itemEntry = Instantiate(itemEditListPrefab, itemEditListContainer);
        Text itemText = itemEntry.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = itemList.listName;
        }

        Button[] buttons = itemEntry.GetComponentsInChildren<Button>();
        if (buttons.Length >= 2)
        {
            Button viewItemsButton = buttons[0];
            if (viewItemsButton != null)
            {
                viewItemsButton.onClick.AddListener(() => EditItemList(itemList));
            }

            Button eraseListButton = buttons[1];
            if (eraseListButton != null)
            {
                eraseListButton.onClick.AddListener(() => EraseList(itemList, itemEntry));
            }
        }
        else
        {
            Debug.LogWarning("Expected two buttons but found less.");
        }
    }

    public void AddItemToCurrentList()
    {
        string itemName = itemNameInputField.text;
        string itemDescription = itemDescriptionInputField.text;

        // Get the image path from the CameraManager (assuming you have a reference to CameraManager)
        string itemImagePath = cameraManager.capturedPhotoPath; // Access the saved photo path

        // Attempt to parse the price input to a float
        float price;
        bool isValidPrice = float.TryParse(itemPriceInputField.text, out price);
        if (!isValidPrice)
        {
            Debug.LogWarning("Invalid price input. Please enter a valid numeric value.");
            return;
        }


        if (currentItemList != null && !string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemImagePath))
        {
            // Create a new item with the parsed price and the PNG path
            Item newItem = new Item(itemName, itemDescription, price, itemImagePath); // Pass the image path to the item constructor
            currentItemList.items.Add(newItem);

            Debug.Log("Item added: " + itemName + " - " + itemDescription + " - Price: " + price + " - Image Path: " + itemImagePath);

            // Clear input fields after adding the item
            itemNameInputField.text = "";
            itemDescriptionInputField.text = "";
            itemPriceInputField.text = ""; // Clear price input field

            // Save the item lists after adding an item
            savingManager.SaveItemLists(itemLists, itemListsShareable);
        }
        else
        {

            Debug.LogWarning("Item name cannot be empty, no list is selected, or no image path is provided.");
        }
    }


    public void FinalizeCurrentList()
    {
        if (currentItemList != null)
        {
            Debug.Log("Finalized list: " + currentItemList.listName + " with " + currentItemList.items.Count + " items.");
            itemCreationPanel.SetActive(false);
            currentItemList = null;

            // Save the item lists after finalizing
            savingManager.SaveItemLists(itemLists, itemListsShareable);
        }
        else
        {
            Debug.LogWarning("No list to finalize.");
        }
    }

    public void ViewItemList(ItemList itemList)
    {
        itemListDisplayPanel.SetActive(true);
        itemListTitleText.text = itemList.listName;

        foreach (Transform child in itemDisplayContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemList.items)
        {
            GameObject itemEntry = Instantiate(itemEntryPrefab, itemDisplayContainer);
            Text itemText = itemEntry.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = item.itemName;
            }
        }
    }

    public void EditItemList(ItemList itemList)
    {
        itemEditListDisplayPanel.SetActive(true);
        itemListTitleText.text = itemList.listName;

        // Clear existing items in the container
        foreach (Transform child in itemEditDisplayContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemList.items)
        {
            GameObject itemEntry = Instantiate(itemEditEntryPrefab, itemEditDisplayContainer);
            Text itemText = itemEntry.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = item.itemName;
            }

            // Retrieve both edit and erase buttons in the item entry
            Button[] buttons = itemEntry.GetComponentsInChildren<Button>();
            if (buttons.Length >= 2)
            {
                Button editItemButton = buttons[0];
                if (editItemButton != null)
                {
                    editItemButton.onClick.AddListener(() => EditingItem(item));
                }

                Button eraseItemButton = buttons[1];
                if (eraseItemButton != null)
                {
                    eraseItemButton.onClick.AddListener(() => EraseItem(item, itemEntry, itemList));
                }
            }
            else
            {
                Debug.LogWarning("Expected two buttons but found less in item entry prefab.");
            }
        }
    }

    private void EraseItem(Item item, GameObject itemEntry, ItemList itemList)
    {
        // Remove the item from the list
        itemList.items.Remove(item);

        // Destroy the UI entry for this item
        Destroy(itemEntry);

        // Save the item lists after deletion
        savingManager.SaveItemLists(itemLists, itemListsShareable);

        Debug.Log("Item deleted: " + item.itemName);
    }

    private void EraseList(ItemList itemList, GameObject itemEntry)
    {
        // Remove the list from the item lists
        itemLists.Remove(itemList);

        // Destroy the UI entry for this list
        Destroy(itemEntry);

        // Save the item lists after deletion
        savingManager.SaveItemLists(itemLists, itemListsShareable);

        Debug.Log("List deleted: " + itemList.listName);
    }

    public void EditingItem(Item item)
    {
        panelEditItem.SetActive(true);
        itemNameDefault.text = item.itemName;
        itemDescriptionDefault.text = item.itemDescription;
        itemPriceDefault.text = item.price.ToString("F2");
    }

    public void SaveEditedItem()
    {
        string editedName = itemNameDefault.text;
        string editedDescription = itemDescriptionDefault.text;

        // Attempt to parse the price input to a float
        float price;
        bool isValidPrice = float.TryParse(itemPriceDefault.text, out price);
        if (!isValidPrice)
        {
            Debug.LogWarning("Invalid price input. Please enter a valid numeric value.");
            return;
        }

        itemNameDefault.text = ""; // Clear input fields

        // Save the edited item details
        if (currentItemList != null)
        {
            Item editedItem = currentItemList.items.Find(i => i.itemName == editedName);
            if (editedItem != null)
            {
                editedItem.itemDescription = editedDescription;
                editedItem.price = price;
                Debug.Log("Item edited: " + editedName);
            }
            else
            {
                Debug.LogWarning("Item not found for editing.");
            }
        }

        panelEditItem.SetActive(false);
        savingManager.SaveItemLists(itemLists, itemListsShareable);
    }


    public void OpenCamera()
    {
        cameraManager.StartCamera();
    }

}
