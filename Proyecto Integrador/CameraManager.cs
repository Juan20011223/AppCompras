using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class CameraManager : MonoBehaviour
{
    public RawImage previewImage;       // Displays the live camera feed
    public GameObject photoPanel;      // Panel that contains the camera UI

    private Texture2D capturedPhoto;   // Stores the captured photo
    public RawImage capturedPhotoImage; // Displays the captured photo

    public Button takePhotoButton;     // Button to take a photo
    public Button backButton;          // Button to return to the previous screen

    public InputField itemNameInputField; // Input field for naming the photo
    public string capturedPhotoPath;   // Path where the photo is saved

    public Camera arCamera; // The AR camera used for the screenshot capture

    private void Start()
    {
        // Hide the photo panel initially
        photoPanel.gameObject.SetActive(false);

        // Set button listeners
        takePhotoButton.onClick.AddListener(() => CapturePhoto(itemNameInputField.text));
        backButton.onClick.AddListener(BackToPreviousScreen);
    }

    public void StartCamera()
    {
        // Activate the AR camera and photo panel
        arCamera.gameObject.SetActive(true);
        photoPanel.gameObject.SetActive(true);
    }

    public void StopCamera()
    {
        // Deactivate the AR camera and photo panel
        //arCamera.gameObject.SetActive(false);
        photoPanel.gameObject.SetActive(false);
    }

    public void CapturePhoto(string itemName)
    {
        // Capture screenshot of the AR camera's view
        StartCoroutine(CapturePhotoCoroutine(itemName));
    }

    private IEnumerator CapturePhotoCoroutine(string itemName)
    {
        // Wait until the frame is fully rendered
        yield return new WaitForEndOfFrame();

        try
        {
            // Debugging: Check if AR Camera is rendering
            if (arCamera == null)
            {
                Debug.LogError("AR Camera is null.");
                yield break;
            }

            // Create a RenderTexture with standard resolution (you can tweak it)
            int renderWidth = Screen.width;
            int renderHeight = Screen.height;
            RenderTexture renderTexture = new RenderTexture(renderWidth, renderHeight, 24);

            // Apply the render texture to the AR camera
            arCamera.targetTexture = renderTexture;

            // Render the AR camera's view to the RenderTexture
            arCamera.Render();

            // Create a new Texture2D with RGB24 format (you can change to ARGB32 if required)
            capturedPhoto = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

            // Copy the pixels from the RenderTexture to the Texture2D
            RenderTexture.active = renderTexture;
            capturedPhoto.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            capturedPhoto.Apply();

            // Display the captured photo in the UI
            capturedPhotoImage.texture = capturedPhoto;
            capturedPhotoImage.gameObject.SetActive(true);

            // Save the captured photo
            SavePhoto(capturedPhoto, itemName);

            // Reset the AR camera's target texture to avoid interrupting AR rendering
            arCamera.targetTexture = null;
            RenderTexture.active = null;

            // Debugging: Confirm that the photo was captured and AR camera reset
            Debug.Log("Captured photo and reset AR camera target texture.");

        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to capture photo: {ex.Message}");
        }
    }



    private void SavePhoto(Texture2D photo, string itemName)
    {
        try
        {
            // Encode the photo to PNG format
            byte[] bytes = photo.EncodeToPNG();

            // Generate a unique filename using the item name and timestamp
            string fileName = $"{itemName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            capturedPhotoPath = Path.Combine(Application.persistentDataPath, fileName);

            // Save the photo to the file system
            File.WriteAllBytes(capturedPhotoPath, bytes);

            Debug.Log($"Photo saved to: {capturedPhotoPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save photo: {ex.Message}");
        }
    }

    public void BackToPreviousScreen()
    {
        StopCamera();
    }

    private void OnDestroy()
    {
        StopCamera();
    }
}
