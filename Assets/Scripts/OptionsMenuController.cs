using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsPanel;  // Panel del men� de opciones
    public Button backButton;  // Bot�n para volver del men� de opciones
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    private void Start()
    {
        // Registrar los sliders en el AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterVolumeSliders(masterVolumeSlider, sfxVolumeSlider, musicVolumeSlider);
        }

        // Asignar el listener del bot�n de volver
        backButton.onClick.AddListener(OnBackButtonClicked);
        // Asegurarse de que el panel de opciones est� desactivado al inicio
        optionsPanel.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        optionsPanel.SetActive(false);
        // Aqu� puedes activar el panel del men� principal si es necesario
    }

    public void ShowOptionsMenu()
    {
        optionsPanel.SetActive(true);
    }
}

