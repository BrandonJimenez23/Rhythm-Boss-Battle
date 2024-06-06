using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField]
    public float activationTime; // Tiempo en la canción en el que se activa la sección
    private bool isActivated; // Indica si la sección ha sido activada
    private bool isActivatedNotified; // Indica si la sección ha sido notificada de su activación
    private AudioSource song;
    public float activationOffset = 1f; // Offset de activación en segundos antes del tiempo de activación de la sección

    void Start()
    {
        // Calcula el tiempo de activación con el offset
        float actualActivationTime = activationTime - activationOffset;

        // Activa la sección automáticamente si ya debería estar activada
        song = FindObjectOfType<GameManager>().song;
    
        if (actualActivationTime <= 0)
        {
            Activate();
        }
    }

    void Update()
    {
        // Verifica si la sección debe activarse en este momento
        if (!isActivated && !isActivatedNotified)
        {
            float actualActivationTime = activationTime - activationOffset;
            float currentSongTime = song.time; // Reemplaza AudioManager.Instance.GetSongTime() por el método que obtiene el tiempo actual de la canción

            if (currentSongTime >= actualActivationTime)
            {
                Activate();
            }
        }
    }

    // Método para activar la sección y sus notas
    public void Activate()
    {
        isActivated = true;
        isActivatedNotified = true;

        // Activa todos los hijos del objeto de la sección
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}

