using UnityEngine;
using Vuforia;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;

public class ImageTargetManager : MonoBehaviour
{
    public GameObject arCamera;
    public string jsonFilePath = "savedItems.json";
    private List<Texture2D> itemImageTextures = new List<Texture2D>();

    private List<GameObject> instantiatedImageTargets = new List<GameObject>();

    private ItemList currentItemList;

    public InventoryManager inventoryManager;
    public GameObject canvasPrefab;


    void Start()
    {
        // Ensure Vuforia is initialized before creating image targets
        jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFilePath);
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }

    private void OnVuforiaStarted()
    {
        Debug.Log("Vuforia initialized successfully.");
    }

    private void LoadImagesFromJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            ItemListDataWrapper itemListDataWrapper = JsonUtility.FromJson<ItemListDataWrapper>(jsonData);

            foreach (var itemList in itemListDataWrapper.itemLists)
            {
                Debug.Log("List Name: " + itemList.listName);

                foreach (var item in itemList.items)
                {
                    Debug.Log("Item Name: " + item.itemName);
                    Debug.Log("Item Description: " + item.itemDescription);
                    Debug.Log("Item Price: " + item.price);
                    Debug.Log("Item Image Path: " + item.imagePath);

                    // Load the image for each item
                    CreateImageTargetFromPath(item.imagePath, item.itemName, item.price, itemList.listName);
                }
            }
        }
        else
        {
            Debug.LogError("JSON file not found at: " + jsonFilePath);
        }
    }

    public void SetItemList(ItemList itemList)
    {
        // Use the inventoryManager's CompareList method to find a unique name
        string baseName = itemList.listName;
        string uniqueName = baseName;

        // Use a for loop to find a unique name
        for (int counter = 1; ; counter++)
        {
            // If the name is unique, break out of the loop
            if (!inventoryManager.CompareList(uniqueName))
            {
                break;
            }
            else
            {
                // Otherwise, append the counter to the base name and try again
                uniqueName = baseName + counter.ToString();
            }
        }

        // Set the current item list for processing
        currentItemList = itemList;
        Debug.Log("Item list set for scanning: " + uniqueName); // Use original name in logs

        // Process the items in the list, passing the unique name to the function
        inventoryManager.CreateNewShareableItemList(uniqueName,baseName);
        foreach (var item in currentItemList.items)
        {
            CreateImageTargetFromPath(item.imagePath, item.itemName, item.price, uniqueName);
        }
    }
    private void CreateImageTargetFromPath(string path, string itemName, float price, string currentlist)
    {
        if (File.Exists(path))
        {
            // Load the image bytes from the file
            byte[] imageBytes = File.ReadAllBytes(path);
            Texture2D itemImageTexture = new Texture2D(2, 2);  // Create a new Texture2D
            bool isImageLoaded = itemImageTexture.LoadImage(imageBytes);

            if (!isImageLoaded)
            {
                Debug.LogError($"Failed to load texture for item: {itemName} at path: {path}");
                return;
            }

            // Creating the image target without using a prefab
            float targetWidth = 0.2f; // Set the default target width
            var imageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                itemImageTexture,
                targetWidth,
                itemName);

            if (imageTarget == null)
            {
                Debug.LogError($"Failed to create Image Target for {itemName}. ImageTarget is null.");
                return;
            }


            // Instantiate the Canvas prefab and set it as a child of the image target
            if (canvasPrefab != null)
            {
                // Instantiate the Canvas prefab and set it as a child of the image target
                GameObject instantiatedCanvas = Instantiate(canvasPrefab, imageTarget.transform);

                // Ensure the Canvas is positioned correctly (adjust as necessary)
                instantiatedCanvas.transform.localPosition = Vector3.zero; // Center it within the target
                instantiatedCanvas.transform.localRotation = Quaternion.identity; // No rotation

                // Optionally adjust scale of the Canvas or UI elements inside it
                instantiatedCanvas.transform.localScale = Vector3.one; // Set scale to 1 (default)

                // Now find and assign the necessary text components inside the canvas
                Transform secondPanelTransform = instantiatedCanvas.transform.GetChild(0).transform.GetChild(1); // Assuming second child is the panel

                if (secondPanelTransform != null)
                {
                    Button[] textComponents = secondPanelTransform.GetComponentsInChildren<Button>();
                    if (textComponents.Length >= 3)
                    {
                        // Assign values to the Text components
                        textComponents[0].GetComponentInChildren<Text>().text = itemName; // Item Name
                        textComponents[1].GetComponentInChildren<Text>().text = price.ToString(); // Item Price
                        textComponents[2].GetComponentInChildren<Text>().text = currentlist; // Current List Name
                        Debug.Log("Set itemName, price, and currentlist inside prefab Text components.");
                    }
                    else
                    {
                        Debug.LogError("Text components for itemName, price, and currentlist not found in second panel.");
                    }
                }
                else
                {
                    Debug.LogError("Second panel not found inside the Canvas.");
                }
            }
            else
            {
                Debug.LogError("Canvas prefab is null.");
            }

            // Track the created image target
            instantiatedImageTargets.Add(imageTarget.gameObject);
            Debug.Log($"Image target for {itemName} successfully created.");
        }
        else
        {
            Debug.LogError($"Image file not found at path: {path}");
        }
    }



    private void OnDestroy()
    {
        // Clean up instantiated image targets to avoid accessing destroyed objects
        foreach (var target in instantiatedImageTargets)
        {
            if (target != null) // Null check before accessing the object
            {
                var observer = target.GetComponent<ObserverBehaviour>();
                if (observer != null)
                {
                    observer.OnTargetStatusChanged -= OnTargetStatusChanged;
                }
                Destroy(target);
            }
        }
    }

    public void DestroyAllImageTargets()
    {
        // Iterate through the list of instantiated image targets
        foreach (var target in instantiatedImageTargets)
        {
            if (target != null) // Check if the target is still valid
            {
                Destroy(target); // Destroy the GameObject
            }
        }

        // Clear the list after destroying all targets
        instantiatedImageTargets.Clear();

        Debug.Log("All image targets have been destroyed.");
    }

    private void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status)
    {
        // Null check before accessing target components
        if (observer != null && observer.gameObject != null)
        {
            if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
            {
                Debug.Log($"Found Image Target for item: {observer.TargetName}");
            }
        }
    }
}

[Serializable]
public class ItemListDataWrapper
{
    public List<ItemList> itemLists;

    public ItemListDataWrapper(List<ItemList> itemLists)
    {
        this.itemLists = itemLists;
    }
}
