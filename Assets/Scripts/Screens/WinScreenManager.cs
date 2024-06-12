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
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        string currentWeek = PlayerPrefs.GetString("CurrentWeek");

        // Actualizar los datos del nivel
        LevelProgress levelProgress = ProgressManager.instance.GetLevelProgress(currentWeek, currentLevel);
        if (levelProgress != null)
        {
            levelProgress.score = score;
            levelProgress.misses = misses;
            levelProgress.sicks = sicks;
            levelProgress.goods = goods;
            levelProgress.isCompleted = true;
            // Verificar si es un nuevo high score
            levelProgress.isNewHighScore = score > levelProgress.score;
        }

        // Guardar el progreso
        ProgressManager.instance.SaveProgress();

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
        string currentWeek = PlayerPrefs.GetString("CurrentWeek");

        // Determinar el siguiente nivel
        string nextLevel = GetNextLevel(currentWeek, currentLevel);

        // Cargar el siguiente nivel o el menú principal
        if (!string.IsNullOrEmpty(nextLevel))
        {
            PlayerPrefs.SetString("CurrentLevel", nextLevel);
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene("LevelSelectionMenu");
        }
    }

    private string GetNextLevel(string currentWeek, string currentLevel)
    {
        WeekProgress weekProgress = ProgressManager.instance.GetWeekProgress(currentWeek);
        if (weekProgress != null)
        {
            int currentIndex = weekProgress.levels.FindIndex(level => level.levelName == currentLevel);
            if (currentIndex >= 0 && currentIndex < weekProgress.levels.Count - 1)
            {
                return weekProgress.levels[currentIndex + 1].levelName;
            }
            else
            {
                // Completar la semana y desbloquear la siguiente
                weekProgress.isCompleted = true;
                ProgressManager.instance.SaveProgress();
                return null;
            }
        }
        return null;
    }
}

