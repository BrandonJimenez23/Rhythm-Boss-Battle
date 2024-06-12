using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public AudioSource introSong;
    public AudioSource sfx;
    public AudioClip buttonClickSound;
    public Button playButton;
    public Button exitButton;
    public Button optionsButton;
    public Button backButton;  // Botón para volver del menú de opciones
    public GameObject menuPanel;  // Panel del menú principal
    public GameObject optionsMenuPrefab;  // Panel del menú de opciones

    private void Start()
    {
        introSong.Play();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        backButton.onClick.AddListener(HideOptionsMenu);
        exitButton.onClick.AddListener(OnExitButtonClicked); // Desactivarlo al inicio
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        AudioManager.Instance.RestoreVolumes();

    }

    private void OnPlayButtonClicked()
    {
        sfx.PlayOneShot(buttonClickSound);
        StartCoroutine(WaitAndLoadScene("LevelSelectionMenu"));
    }

    private void OnExitButtonClicked()
    {
        sfx.PlayOneShot(buttonClickSound);
        StartCoroutine(WaitAndQuit());
    }
    private void ShowOptionsMenu()
    {
        sfx.PlayOneShot(buttonClickSound);
        gameObject.SetActive(false);
        optionsMenuPrefab.SetActive(true);
    }
    private void HideOptionsMenu()
    {
        sfx.PlayOneShot(buttonClickSound);
        gameObject.SetActive(true);
        optionsMenuPrefab.SetActive(false);
    }

    private IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(buttonClickSound.length);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(buttonClickSound.length);
        Application.Quit();
    }
}
