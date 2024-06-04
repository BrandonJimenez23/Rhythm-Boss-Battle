using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public float bpm;
    private float beatInterval;
    private float nextBeatTime;

    void Start()
    {
        beatInterval = 60f / bpm;
        nextBeatTime = beatInterval;
    }

    void Update()
    {
        if (audioSource.isPlaying && audioSource.time >= nextBeatTime)
        {
            nextBeatTime += beatInterval;
            OnBeat();
        }
    }

    public void OnBeat()
    {
        // Aquí llamas a la función para mover la cámara
        CameraController.Instance.OnBeat();
    }
}

