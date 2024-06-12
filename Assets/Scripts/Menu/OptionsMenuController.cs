using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsPanel;  // Panel del menú de opciones
    public Button backButton;  // Botón para volver del menú de opciones
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

        // Asignar el listener del botón de volver
        backButton.onClick.AddListener(OnBackButtonClicked);
        // Asegurarse de que el panel de opciones esté desactivado al inicio
        optionsPanel.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        optionsPanel.SetActive(false);
        // Aquí puedes activar el panel del menú principal si es necesario
    }

    public void ShowOptionsMenu()
    {
        optionsPanel.SetActive(true);
    }
}

