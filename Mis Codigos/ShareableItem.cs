using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareableItem : MonoBehaviour
{
    // List to store images
    public List<Sprite> logoList;
    public Image img1;
    public Image img2;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure there are at least two images in the logoList
        if (logoList.Count < 2)
        {
            Debug.LogError("Not enough images in the logoList to assign to img1 and img2.");
            return;
        }

        // Shuffle the list and select the first two images
        List<Sprite> shuffledList = new List<Sprite>(logoList);
        Shuffle(shuffledList);

        // Assign the selected images to img1 and img2
        img1.sprite = shuffledList[0];
        img2.sprite = shuffledList[1];

        Debug.Log($"Assigned images: img1={shuffledList[0].name}, img2={shuffledList[1].name}");
    }

    // Utility function to shuffle a list
    private void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
