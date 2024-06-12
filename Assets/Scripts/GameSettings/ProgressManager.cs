using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;
    public GameProgress gameProgress;
    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "gameProgress.json");
        LoadProgress();
    }

    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(gameProgress, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            gameProgress = JsonUtility.FromJson<GameProgress>(json);
        }
        else
        {
            InitializeNewProgress();
        }
    }

    private void InitializeNewProgress()
    {
        gameProgress = new GameProgress
        {
            weeks = new List<WeekProgress>
            {
                new WeekProgress
                {
                    weekName = "Week 1",
                    weekImage = "Assets/Images/Weeks/Week1.png", // Path to the image in Resources
                    trackListPrefab = "Week1TrackListPrefab", // Path to the TrackList prefab in Resources
                    levels = new List<LevelProgress>
                    {
                        new LevelProgress { levelName = "Level 1-1" },
                        new LevelProgress { levelName = "Level 1-2" },
                        new LevelProgress { levelName = "Level 1-3" }
                    },
                    tracks = new List<Track>
                    {
                        new Track { name = "HEX-RAM", difficulty = "Easy" },
                        new Track { name = "HEX-HELLOWORLD", difficulty = "Normal" },
                        new Track { name = "HEX-COOLING", difficulty = "Hard" }
                    }
                },
                new WeekProgress
                {
                    weekName = "Week 2",
                    weekImage = "Assets/Images/Weeks/Week2.png", // Path to the image in Resources
                    trackListPrefab = "Week2TrackListPrefab", // Path to the TrackList prefab in Resources
                    levels = new List<LevelProgress>
                    {
                        new LevelProgress { levelName = "Level 2-1" },
                        new LevelProgress { levelName = "Level 2-2" },
                        new LevelProgress { levelName = "Level 2-3" }
                    },
                    tracks = new List<Track>
                    {
                        new Track { name = "???", difficulty = "Nightmare" }
                    }
                }
                // Add more weeks as needed
            }
        };
    }


    public WeekProgress GetWeekProgress(string weekName)
    {
        return gameProgress.weeks.Find(week => week.weekName == weekName);
    }

    public LevelProgress GetLevelProgress(string weekName, string levelName)
    {
        WeekProgress weekProgress = GetWeekProgress(weekName);
        return weekProgress?.levels.Find(level => level.levelName == levelName);
    }

    public bool IsWeekUnlocked(string weekName)
    {
        int weekIndex = gameProgress.weeks.FindIndex(week => week.weekName == weekName);
        if (weekIndex == 0) return true;  // First week is always unlocked
        if (weekIndex > 0 && weekIndex < gameProgress.weeks.Count)
        {
            return gameProgress.weeks[weekIndex - 1].isCompleted;
        }
        return false;
    }
}

