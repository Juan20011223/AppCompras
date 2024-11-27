using System.IO;
using UnityEngine;
using Vuforia;

public class RuntimeImageTarget : MonoBehaviour
{
    public GameObject placeholderPrefab; // Optional: Assign a prefab for visual representation of the target.
    public GameObject cubePrefab; // Optional: Assign a cube prefab to test if image is being scanned.

    void Start()
    {
        // Ensure Vuforia is initialized before creating image targets
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }

    private void OnVuforiaStarted()
    {
        Debug.Log("Vuforia initialized successfully.");
    }

    /// <summary>
    /// Creates an Image Target dynamically from a texture.
    /// </summary>
    /// <param name="image">The texture of the image to use as a target.</param>
    /// <param name="targetName">The name for the new Image Target.</param>
    public void CreateTarget(Texture2D image, string targetName)
    {
        if (image == null || string.IsNullOrEmpty(targetName))
        {
            Debug.LogError("Invalid image or target name.");
            return;
        }

        // Calculate the width of the image target in scene units
        float targetWidth = 0.5f; // Example size in Unity units (adjust as needed).

        // Create the Image Target using the loaded texture
        var imageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
            image,
            targetWidth,
            targetName);

        if (imageTarget != null)
        {
            Debug.Log($"Image Target '{targetName}' created successfully.");

            // Optional: Add a placeholder visual representation
            if (placeholderPrefab != null)
            {
                GameObject instantiatedObject = Instantiate(placeholderPrefab, imageTarget.transform.position, Quaternion.identity);

                // Change the texture of the placeholder prefab to the loaded image
                Renderer renderer = instantiatedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Apply the texture to the placeholder prefab
                    renderer.material.mainTexture = image;
                }
            }

            // Add a cube inside the image target to test if it's scanning
            if (cubePrefab != null)
            {
                GameObject cube = Instantiate(cubePrefab, imageTarget.transform.position, Quaternion.identity);
                cube.transform.SetParent(imageTarget.transform); // Make the cube a child of the image target

                // Position the cube a little above the image target (optional)
                cube.transform.localPosition = new Vector3(0, 0.1f, 0); // Adjust position as necessary
            }

            imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError("Failed to create Image Target.");
        }
    }

    /// <summary>
    /// Loads an image from a file and creates a target.
    /// </summary>
    /// <param name="filePath">The file path of the image.</param>
    /// <param name="targetName">The name for the Image Target.</param>
    public void CreateTargetFromFile(string filePath, string targetName)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        // Load the image into a Texture2D
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D instance
        texture.LoadImage(imageBytes); // Load the image data into the texture

        // After loading the texture, we can create the target
        CreateTarget(texture, targetName);
    }

    /// <summary>
    /// Logs changes in the target's status.
    /// </summary>
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log($"Target '{behaviour.TargetName}' status: {status.Status}");
    }

    public void Creation()
    {
        string filePath = "C:/Users/juanr/AppData/LocalLow/DefaultCompany/ARP Euhphoria/sss_20241114_005846.png";

        CreateTargetFromFile(filePath, "ExampleTarget");
    }
}
