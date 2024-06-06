using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    public AudioSource sfx;
    public AudioClip buttonClickSound;
    public AudioMixer audioMixer;
    public Button backButton;
    public Button optionsButton;
    public GameObject optionsMenuPrefab;
    public GameManager gameManager;

    private void Start()
    {

        // Asigna los listeners a los botones
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(BackToMenu);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameObject.SetActive(false);

    }
    private void Update()
    {
        
    }

    public void Resume()
    {
        // Reanuda el juego
        gameManager.ResumeGame();
        
    }

    public void Pause()
    {
        // Pausa el juego
        gameManager.PauseGame();
        
    }

    public void Restart()
    {
        // Reinicia la escena actual
        gameManager.RestartGame();
    }
    private void ShowOptionsMenu()
    {
        sfx.PlayOneShot(buttonClickSound);
        gameObject.SetActive(false);
        optionsMenuPrefab.SetActive(true);
    }
    public void HideOptionsMenu()
    {
        sfx.PlayOneShot(buttonClickSound);
        gameObject.SetActive(true);
        optionsMenuPrefab.SetActive(false);
    }

    public void BackToMenu()
    {
        // Vuelve al men� principal
        gameManager.BackToMenu();
    }
}
