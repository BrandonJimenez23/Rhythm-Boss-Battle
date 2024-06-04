using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;



public class NoteMapping
{
    public List<Note> Notes { get; set; }

    public void SaveToFile(string path)
    {
        var json = JsonConvert.SerializeObject(this);
        File.WriteAllText(path, json);
    }

    public static NoteMapping LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<NoteMapping>(json);
    }
}

public class NoteRecorder : MonoBehaviour
{
    public string savePath;
    private Dictionary<int, Note> activeNotes = new Dictionary<int, Note>();
    private NoteMapping noteMapping = new NoteMapping { Notes = new List<Note>() };

    void Update()
    {
        string[] keys = { "a", "s", "k", "l" };
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                var note = new Note { Key = i, Time = Time.time, Duration = 0 };
                activeNotes[i] = note;
                noteMapping.Notes.Add(note);
            }
            else if (Input.GetKeyUp(keys[i]) && activeNotes.ContainsKey(i))
            {
                var note = activeNotes[i];
                note.Duration = Time.time - note.Time;
                if (note.Duration < 0.15f)
                {
                    note.Duration = 0;
                }
                activeNotes.Remove(i);
            }
        }
    }
    public void SaveNotes()
    {
        noteMapping.SaveToFile(savePath);
        Debug.Log("Notas guardadas en " + savePath);
    }

    void OnApplicationQuit()
    {
        noteMapping.SaveToFile(savePath);
    }
}
