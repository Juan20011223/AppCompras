using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class ARButtons : MonoBehaviour
{
    // Variable to keep track of the total price (this can still be used, if needed)
    private float totalPrice = 0;
    public Text name;
    public Text price;
    public Text listname;
    public InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();


    }

    // This function is called when the "Anadir" button is pressed
    public void Anadir()
    {
     Item newItem = new Item(name.text, "", float.Parse(price.text), "");
        inventoryManager.AddItemToCurrentList(newItem);
    }

    // This function is called when the "NoAnadir" button is pressed
    public void NoAnadir()
    {
        Debug.Log("No se anadio objeto a la lista");
    }

    // Helper function to get the name of the active target
    private string GetActiveTargetName()
    {
        // Get all currently active Trackables in the scene
        ObserverBehaviour[] observers = FindObjectsOfType<ObserverBehaviour>();

        foreach (ObserverBehaviour observer in observers)
        {
            // Check if the target is tracked
            if (observer.TargetStatus.Status == Status.TRACKED || observer.TargetStatus.Status == Status.EXTENDED_TRACKED)
            {
                // Return the name of the tracked target
                return observer.TargetName;
            }
        }

        // No active target found
        return null;
    }
}
