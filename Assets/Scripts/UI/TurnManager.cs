using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public BeatDetector beatDetector;
    public CameraController cameraController;
    private bool isPlayerTurn = true;

    void Start()
    {
        
    }

    void OnDestroy()
    {
        
    }

    void HandleBeat()
    {
        // Lógica para determinar si es el turno del jugador o del enemigo
        if (isPlayerTurn)
        {
            cameraController.SwitchFocus(true);
        }
        else
        {
            cameraController.SwitchFocus(false);
        }
        isPlayerTurn = !isPlayerTurn;
    }
}

