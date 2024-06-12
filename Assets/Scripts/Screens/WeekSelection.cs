using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class WeekSelectionManager : MonoBehaviour
{
    public GameObject weekPrefab; // Prefab for the week UI
    public Transform weekContainer; // Container for the weeks
    public Button playButton;
    public Button leftArrow;
    public Button rightArrow;
    public Button backButton;
    public AudioSource sfx;
    public AudioClip buttonClickSound;
    private List<WeekProgress> weekProgressList;
    private int currentWeekIndex = 0;

    private void Start()
    {
        // Load game progress
        GameProgress gameProgress = ProgressManager.instance.gameProgress;

        weekProgressList = gameProgress.weeks;

        // Setup initial week
        if (weekProgressList.Count > 0)
        {
            SetupWeek(weekProgressList[currentWeekIndex]);
        }

        leftArrow.onClick.AddListener(ShowPreviousWeek);
        rightArrow.onClick.AddListener(ShowNextWeek);
        playButton.onClick.AddListener(PlayCurrentWeek);
        backButton.onClick.AddListener(BackToMenu);
    }

    private void SetupWeek(WeekProgress weekProgress)
    {
        // Clear existing weeks
        foreach (Transform child in weekContainer)
        {
            Destroy(child.gameObject);
        }

        // Determine if the current week should be locked
        bool isLocked = currentWeekIndex > 0 && !IsPreviousWeekCompleted();

        // Instantiate the week prefab
        GameObject weekObject = Instantiate(weekPrefab, weekContainer);
        WeekUI weekUI = weekObject.GetComponent<WeekUI>();
        weekUI.Setup(weekProgress, isLocked);

        // Store current week and level in PlayerPrefs
        PlayerPrefs.SetString("CurrentWeek", weekProgress.weekName);
        PlayerPrefs.SetString("CurrentLevel", weekProgress.levels[0].levelName);
        PlayerPrefs.Save();

        // Enable or disable play button based on lock status
        playButton.interactable = !isLocked;
    }

    private bool IsPreviousWeekCompleted()
    {
        if (currentWeekIndex > 0)
        {
            WeekProgress previousWeek = weekProgressList[currentWeekIndex - 1];
            int totalLevels = previousWeek.levels.Count;
            int completedLevels = 0;

            foreach (var level in previousWeek.levels)
            {
                if (level.isCompleted)
                {
                    completedLevels++;
                }
            }

            float percentage = (float)completedLevels / totalLevels * 100;
            return Mathf.CeilToInt(percentage) == 100;
        }

        return true; // The first week is always unlocked
    }

    private void BackToMenu()
    {
        sfx.PlayOneShot(buttonClickSound);
        SceneManager.LoadScene("MainMenuScene");
    }
    private void ShowPreviousWeek()
    {
        if (currentWeekIndex > 0)
        {
            currentWeekIndex--;
            SetupWeek(weekProgressList[currentWeekIndex]);
        }
    }

    private void ShowNextWeek()
    {
        if (currentWeekIndex < weekProgressList.Count - 1)
        {
            currentWeekIndex++;
            SetupWeek(weekProgressList[currentWeekIndex]);
        }
    }
    private void PlayCurrentWeek()
    {
        sfx.PlayOneShot(buttonClickSound);
        string selectedWeekName = PlayerPrefs.GetString("CurrentWeek");
        string firstLevelName = PlayerPrefs.GetString("CurrentLevel");

        // Log the selected week and level for debugging
        Debug.Log("Selected Week: " + selectedWeekName);
        Debug.Log("First Level: " + firstLevelName);

        // Load the scene
        SceneManager.LoadScene(firstLevelName);

        // Note: In the next scene, you can retrieve these values from PlayerPrefs to know which week and level to load.
    }
}

