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
    public TextMeshProUGUI completionPercentageText;
    public GameObject lockedOverlay;
    // New UI elements for showing stats and grade
    public TextMeshProUGUI missesText;
    public TextMeshProUGUI sicksText;
    public TextMeshProUGUI goodsText;
    public TextMeshProUGUI gradeText;

    public void Setup(WeekProgress weekProgress, bool isLocked)
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

        DisplayCompletionPercentage(weekProgress);

        if (weekProgress.isCompleted)
        {
            DisplayLevelStatsAndGrade(weekProgress.levels);
        }

        // Show or hide the locked overlay
        lockedOverlay.SetActive(isLocked);
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

    // Calculate and display the completion percentage
    private void DisplayCompletionPercentage(WeekProgress weekProgress)
    {
        int totalLevels = weekProgress.levels.Count;
        int completedLevels = 0;

        foreach (var level in weekProgress.levels)
        {
            if (level.isCompleted)
            {
                completedLevels++;
            }
        }

        float percentage = (float)completedLevels / totalLevels * 100;
        int roundedPercentage = Mathf.CeilToInt(percentage);

        // If roundedPercentage is 100, it means 99% should be displayed as 100%
        if (roundedPercentage == 100 && percentage < 100)
        {
            roundedPercentage = 99;
        }

        completionPercentageText.text = $"{roundedPercentage}%";
    }

    // Calculate and display stats and grade if the week is completed
    private void DisplayLevelStatsAndGrade(List<LevelProgress> levels)
    {
        int totalMisses = 0;
        int totalSicks = 0;
        int totalGoods = 0;
        int totalNotes = 0;
        int totalPlayedNotes = 0;

        foreach (var level in levels)
        {
            if (level.isCompleted)
            {
                totalMisses += level.misses;
                totalSicks += level.sicks;
                totalGoods += level.goods;
                totalPlayedNotes += level.sicks + level.goods + level.misses;
            }
        }

        totalNotes = totalSicks + totalGoods;
        float accuracy = (float)totalNotes / totalPlayedNotes * 100;
        float sickAccuracy = (float)totalSicks / totalNotes * 100;

        missesText.text = $"Misses: {totalMisses}";
        sicksText.text = $"Sicks: {totalSicks}";
        goodsText.text = $"Goods: {totalGoods}";
        gradeText.text = CalculateGrade(accuracy, sickAccuracy);
    }

    // Calculate the grade based on accuracy and sick accuracy
    private string CalculateGrade(float accuracy, float sickAccuracy)
    {
        if (accuracy == 100)
        {
            if (sickAccuracy == 100)
            {
                return "S+";
            }
            return "S";
        }
        else if (accuracy >= 90)
        {
            if (sickAccuracy >= 90)
            {
                return "A+";
            }
            return "A";
        }
        else if (accuracy >= 80)
        {
            if (sickAccuracy >= 80)
            {
                return "B+";
            }
            return "B";
        }
        else if (accuracy >= 70)
        {
            if (sickAccuracy >= 70)
            {
                return "C+";
            }
            return "C";
        }
        else if (accuracy >= 60)
        {
            if (sickAccuracy >= 60)
            {
                return "D+";
            }
            return "D";
        }
        else
        {
            return "F";
        }
    }
}
