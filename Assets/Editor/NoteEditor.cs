using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoteObject))]
public class NoteEditor : Editor
{
    public float fallSpeed = 800f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Obtenemos una referencia al script NoteObject
        NoteObject noteObject = (NoteObject)target;

        // A�adimos un bot�n para calcular el tiempo de pulsaci�n
        if (GUILayout.Button("Calculate time"))
        {
            noteObject.CalculatePressTime(fallSpeed);
        }
    }
}
