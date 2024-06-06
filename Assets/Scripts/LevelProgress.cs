using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Track
{
    public string name;
    public string difficulty;
}

[System.Serializable]
public class LevelProgress
{
    public string levelName;
    public int score;
    public int misses;
    public int sicks;
    public int goods;
    public int timesPlayed;
    public bool isCompleted;
    public bool isNewHighScore;
     // Añadir una lista de canciones
}

[System.Serializable]
public class WeekProgress
{
    public string weekName;
    public string weekImage;
    public List<LevelProgress> levels;
    public string trackListPrefab;
    public bool isCompleted;
    public List<Track> tracks;
}

[System.Serializable]
public class GameProgress
{
    public List<WeekProgress> weeks = new List<WeekProgress>();
}

