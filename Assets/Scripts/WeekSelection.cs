using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class WeekSelectionManager : MonoBehaviour
{
    public GameObject weekPrefab; // Prefab for the week UI
    public Transform weekContainer; // Container for the weeks
    public Button leftArrow;
    public Button rightArrow;

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
    }

    private void SetupWeek(WeekProgress weekProgress)
    {
        // Clear existing weeks
        foreach (Transform child in weekContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the week prefab
        GameObject weekObject = Instantiate(weekPrefab, weekContainer);
        WeekUI weekUI = weekObject.GetComponent<WeekUI>();
        weekUI.Setup(weekProgress);
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
}

