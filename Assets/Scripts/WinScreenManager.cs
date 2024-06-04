using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missesText;
    public TextMeshProUGUI sicksText;
    public TextMeshProUGUI goodsText;
    public Button nextLevelButton;

    private void Start()
    {
        // Recuperar los datos de PlayerPrefs
        int score = PlayerPrefs.GetInt("Score");
        int misses = PlayerPrefs.GetInt("Misses");
        int sicks = PlayerPrefs.GetInt("Sicks");
        int goods = PlayerPrefs.GetInt("Goods");


        // Mostrar los datos en los textos
        scoreText.text = "" + score;
        missesText.text = "" + misses;
        sicksText.text = "" + sicks;
        goodsText.text = "" + goods;

        // Agregar listener al botón de siguiente nivel
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void Update()
    {
        // Si se presiona la tecla Enter, volver al menú después de un segundo
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnNextLevelButtonClicked();
        }
    }

    private void OnNextLevelButtonClicked()
    {
        // Obtener el nombre del nivel actual
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");

        // Determinar el siguiente nivel
        string nextLevel = GetNextLevel(currentLevel);

        // Cargar el siguiente nivel o el menú principal
        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    private string GetNextLevel(string currentLevel)
    {
        // Aquí defines la lógica para determinar el siguiente nivel basado en el nivel actual
        switch (currentLevel)
        {
            case "Level1":
                return "Level2";
            case "Level2":
                return "MainMenuScene";
            // Agrega más casos según tus niveles
            default:
                return "MainMenuScene";
        }
    }

    private IEnumerator ReturnToMenu()
    {
        // Esperar un segundo
        yield return new WaitForSeconds(1f);

        // Volver a la escena del menú
        SceneManager.LoadScene("MainMenuScene");
    }
}
