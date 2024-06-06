using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Obtenemos una referencia al script GameController
        GameManager gameController = (GameManager)target;

        // A�adimos un bot�n para generar las notas del jugador
        if (GUILayout.Button("Generate Notes"))
        {
            gameController.GenerateNotes();
        }

        // A�adimos un bot�n para generar las notas del enemigo
        if (GUILayout.Button("Generate Enemy Notes"))
        {
            gameController.GenerateEnemyNotes();
        }
    }
}
