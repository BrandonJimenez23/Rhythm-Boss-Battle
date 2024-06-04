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

        // Añadimos un botón para calcular el tiempo de pulsación
        if (GUILayout.Button("Calculate time"))
        {
            noteObject.CalculatePressTime(fallSpeed);
        }
    }
}
