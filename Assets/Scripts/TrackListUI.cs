using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TrackListUI : MonoBehaviour
{
    public TextMeshProUGUI trackNameText; // Texto para el nombre de la pista
    public Image trackDifficultyImage; // Imagen para la dificultad de la pista
    public RectTransform[] trackContainers; // Contenedores para las pistas
    public Sprite[] difficultySprites; // Sprites para las diferentes dificultades

    public void Setup(List<Track> tracks)
    {
        // Limpiar pistas existentes
        foreach (RectTransform container in trackContainers)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }

        // Instanciar y configurar pistas
        for (int i = 0; i < Mathf.Min(tracks.Count, trackContainers.Length); i++)
        {
            Track track = tracks[i];
            RectTransform container = trackContainers[i];

            // Configurar el nombre de la pista
            TextMeshProUGUI trackName = Instantiate(trackNameText, container);
            trackName.text = track.name;

            // Configurar la imagen de la dificultad de la pista
            Image trackDifficulty = Instantiate(trackDifficultyImage, container);

            // Suponiendo que el campo 'difficulty' contiene el índice del sprite en el array difficultySprites
            int difficultyIndex = 0; // Suponiendo que Easy es el primer sprite, Normal el segundo, Hard el tercero, etc.
            switch (track.difficulty.ToLower())
            {
                case "easy":
                    difficultyIndex = 0;
                    break;
                case "normal":
                    difficultyIndex = 1;
                    break;
                case "hard":
                    difficultyIndex = 2;
                    break;
                case "nightmare":
                    difficultyIndex = 3;
                    break;
                // Agrega más casos según sea necesario para otras dificultades
            }

            trackDifficulty.sprite = difficultySprites[difficultyIndex];
        }
    }
}
