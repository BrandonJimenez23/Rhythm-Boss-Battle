using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class WeekUI : MonoBehaviour
{
    public Image weekImage; // Image for the week name
    public GameObject trackListPrefab; // Prefab for the track list
    public RectTransform trackListContainer; // Container for the track list

    public void Setup(WeekProgress weekProgress)
    {
        // Load the texture from file
        Texture2D texture = LoadTextureFromFile(weekProgress.weekImage);
        if (texture != null)
        {
            // Create a sprite from the texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // Assign the sprite to the image
            weekImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load week image: " + weekProgress.weekImage);
        }

        // Instantiate track list prefab as child of WeekUI
        GameObject trackListObject = Instantiate(trackListPrefab, trackListContainer);

        // Get the TrackListUI component from the instantiated object
        TrackListUI trackListUI = trackListObject.GetComponent<TrackListUI>();

        // Call the Setup method of TrackListUI to configure it with the tracks data
        trackListUI.Setup(weekProgress.tracks);
    }

    // Load texture from file
    private Texture2D LoadTextureFromFile(string filePath)
    {
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image data into the texture
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }

        return texture;
    }
}
