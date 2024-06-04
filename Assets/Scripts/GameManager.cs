using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LongNoteObject[] longNotePrefab;
    public OpponentLongNoteObject opponentLongNotePrefab;
    public AudioSource song;
    public AudioSource voices;
    public AudioSource sfxSource;
    public AudioClip countdownSound1;
    public AudioClip countdownSound2;
    public AudioClip countdownSound3;
    public AudioClip failSound;
    public AudioClip introGo;
    public AudioMixer mixer;
    public Canvas canvas;
    public int currentScore;
    private int missesCount;
    private int sicksCount;
    private int goodsCount;
    public RectTransform playerIcon;
    public RectTransform enemyIcon;
    public GameObject scoreText;
    public GameObject missesText;
    public float playerHealth = 0.5f;
    public float enemyHealth = 0.5f;
    public BeatScroller BS;
    public GameObject contenedor;
    public Transform[] noteSpawnPoints;
    public Transform[] enemySpawnPoints;
    private List<Note> playerNotes;
    private List<Note> opponentNotes;
    public GameObject imagePrefab;
    public Animator BoyFriendAnimator;
    public OpponentNoteObject opponentNotePrefab;
    private List<GameObject> imagePool;
    public NoteObject[] notePrefabs;
    private int nextNoteIndex;
    private int nextOppNoteIndex;
    private float playerIconPosition;
    private float enemyIconPosition;
    private bool isPaused;
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public Sprite[] numberSprites;
    private Image[] comboImages;
    private int comboCount;
    public WinScreenManager winScreenManager;
    public PauseMenuManager pauseMenuManager;
    public ParticleController particleController;
    public Camera mainCamera;
    private Animator cameraAnimator;
    // Nombres de archivo JSON para notas del jugador y del oponente
    public string playerNotesFile;
    public string opponentNotesFile;
    public Button backButton;
    void Start()
    {
        InitializeGame();
        
    }

    void Update()
    {
        UpdateUI();
        if (!song.isPlaying && BS.hasStarted && playerHealth > 0 && !isPaused)
        {
            ShowWinScreen();
        }
    }

    void InitializeGame()
    {
        backButton.onClick.AddListener(BackButton);
        countdownSound1.LoadAudioData();
        countdownSound2.LoadAudioData();
        countdownSound3.LoadAudioData();
        failSound.LoadAudioData();
        introGo.LoadAudioData();
        nextNoteIndex = 0;
        nextOppNoteIndex = 0;
        isPaused = false;
        comboCount = 0;
        missesCount = 0;
        sicksCount = 0;
        goodsCount = 0; 
        song.Stop();
        voices.Stop();
        song.time = 0;
        voices.time = 0;
        cameraAnimator = mainCamera.GetComponent<Animator>();
        InitializeScoreTexts();
        InitializeImagePool();
        StartCoroutine(Countdown());
    }

    void InitializeScoreTexts()
    {
        Text scoreTextComponent = scoreText.GetComponent<Text>();
        Text missesTextComponent = missesText.GetComponent<Text>();
        playerIconPosition = (playerHealth * 10) - 1;
        enemyIconPosition = ((enemyHealth * 10) - 1);
    }

    void InitializeImagePool()
    {
        imagePool = new List<GameObject>(30);
        for (int i = 0; i < 30; i++)
        {
            GameObject imageObject = Instantiate(imagePrefab, contenedor.transform);
            imageObject.SetActive(false);
            imagePool.Add(imageObject);
        }
    }

    void UpdateUI()
    {
        Text scoreTextComponent = scoreText.GetComponent<Text>();
        Text missesTextComponent = missesText.GetComponent<Text>();
        scoreTextComponent.text = "Score: " + currentScore;
        missesTextComponent.text = "Misses: " + missesCount;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(isPaused)
            {
                
                ResumeGame();
                isPaused = false;
            }
            else
            {
                
                PauseGame();
                isPaused = true;
            }
        }
        UpdateHealthBar();
    }

    public void GenerateNotes()
    {
        playerNotes = NoteMapping.LoadFromFile(playerNotesFile).Notes;
        foreach (Note note in playerNotes)
        {
            if (note.Key >= 0 && note.Key < notePrefabs.Length)
            {
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                Vector3 spawnPosition = new Vector3(noteSpawnPoints[note.Key].position.x, initialPositionY, noteSpawnPoints[note.Key].position.z);
                if(note.Duration != 0)
                {
                    LongNoteObject longNoteObject = Instantiate(longNotePrefab[note.Key],spawnPosition, Quaternion.identity, BS.transform);
                    longNoteObject.time = adjustedTime;
                    longNoteObject.fallSpeed = BS.speed;
                    longNoteObject.duration = note.Duration;
                    longNoteObject.key = note.Key;
                    // Asignar referencias de arrow y tail antes de inicializarlas
                    longNoteObject.arrow = longNoteObject.GetComponentInChildren<ArrowNotePart>();
                    longNoteObject.tail = longNoteObject.GetComponentInChildren<TailNotePart>();

                    longNoteObject.arrow.Initialize(note.Key, adjustedTime);
                    longNoteObject.tail.Initialize(note.Duration, BS.speed, note.Key);
                }
                else
                {
                    NoteObject noteObject = Instantiate(notePrefabs[note.Key], spawnPosition, Quaternion.identity, BS.transform);
                    noteObject.key = note.Key;
                    noteObject.time = adjustedTime;
                    noteObject.offset = countdownSound1.length + countdownSound2.length + countdownSound3.length + introGo.length;
                }
                
                nextNoteIndex++;
            }
            else
            {
                Debug.LogError("Invalid key: " + note.Key);
            }
        }
    }

    public void GenerateEnemyNotes()
    {
        opponentNotes = NoteMapping.LoadFromFile(opponentNotesFile).Notes;
        foreach (Note note in opponentNotes)
        {
            if (note.Key >= 0 && note.Key < notePrefabs.Length)
            {
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                Vector3 spawnPosition = new Vector3(enemySpawnPoints[note.Key].position.x, initialPositionY, enemySpawnPoints[note.Key].position.z);
                if (note.Duration != 0)
                {
                    OpponentLongNoteObject longNoteObject = Instantiate(opponentLongNotePrefab, spawnPosition, Quaternion.identity, BS.transform);
                    longNoteObject.fallSpeed = BS.speed;
                    longNoteObject.duration = note.Duration;
                    longNoteObject.key = note.Key;

                    // Asignar referencias de arrow y tail antes de inicializarlas
                    longNoteObject.arrow = longNoteObject.GetComponentInChildren<OpponentArrowNotePart>();
                    longNoteObject.tail = longNoteObject.GetComponentInChildren<OpponentTailNotePart>();

                    longNoteObject.arrow.Initialize(note.Key, BS.speed);
                    longNoteObject.tail.Initialize(note.Duration, BS.speed, note.Key);
                }
                else
                {
                    OpponentNoteObject noteObject = Instantiate(opponentNotePrefab, spawnPosition, Quaternion.identity, BS.transform);
                    noteObject.SetSprite(note.Key);
                }
            }
            else
            {
                Debug.LogError("Invalid key: " + note.Key);
            }
        }
    }

    public void StartSong()
    {
        song.Play();
        voices.Play();
        missesCount = 0;
        currentScore = 0;
    }
    public void PauseAudio()
    {
        song.Pause();
        voices.Pause();
        isPaused = true;
        // SFX no se pausa para mantener los sonidos de botón
    }
    public void ResumeAudio()
    {
        song.UnPause();
        voices.UnPause();
        isPaused = false;
    }
    public void PauseGame()
    {
        
        PauseAudio();
        pauseMenuManager.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        ResumeAudio();
        pauseMenuManager.HideOptionsMenu();
        pauseMenuManager.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        pauseMenuManager.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void BackButton()
    {
        pauseMenuManager.gameObject.SetActive(true);
    }

    public void HitNotePerfect(int buttonIndex)
    {
        currentScore += 100;
        comboCount++;
        playerHealth = Mathf.Clamp(playerHealth + 0.01f, 0, 1);
        playerIconPosition = Mathf.Clamp(playerIconPosition - 1f, -42, 50);
        enemyIconPosition = Mathf.Clamp(enemyIconPosition + 1f, -42, 50);
        ShowFeedbackImage("ShowPerfect");
        // Reproducir partículas en el botón correspondiente
        particleController.PlayParticles(buttonIndex);


    }

    public void HitNoteSick()
    {
        currentScore += 50;
        comboCount++;
        playerHealth = Mathf.Clamp(playerHealth + 0.01f, 0, 1);
        playerIconPosition = Mathf.Clamp(playerIconPosition - 1f, -42, 50);
        enemyIconPosition = Mathf.Clamp(enemyIconPosition + 1f, -42, 50);
    }

    public void HitNoteGood()
    {
        currentScore += 25;
        comboCount++;
        playerHealth = Mathf.Clamp(playerHealth + 0.01f, 0, 1);
        playerIconPosition = Mathf.Clamp(playerIconPosition - 1f, -42, 50);
        enemyIconPosition = Mathf.Clamp(enemyIconPosition + 1f, -42, 50);
        ShowFeedbackImage("ShowGood");
    }

    public void MissNote()
    {
        currentScore = Mathf.Max(currentScore - 10, 0);
        comboCount = 0;
        missesCount++;
        enemyHealth = Mathf.Clamp(enemyHealth + 0.02f, 0, 1);
        playerHealth = Mathf.Clamp(playerHealth - 0.02f, 0, 1);
        playerIconPosition = Mathf.Clamp(playerIconPosition + 2f, -42, 50);
        enemyIconPosition = Mathf.Clamp(enemyIconPosition - 2f, -42, 50);
        ShowFeedbackImage("ShowBad");
        HandleFail();
    }

    public void LongNoteScore(int score)
    {
        currentScore += score;
    }
   public void HandleFail()
    {
        BoyFriendAnimator.SetTrigger("Fail");
        sfxSource.PlayOneShot(failSound);
        StartCoroutine(RestoreVoicesVolumeForShortDuration());
        BoyFriendAnimator.SetLayerWeight(0, 0);
        BoyFriendAnimator.SetLayerWeight(1, 0);
        BoyFriendAnimator.SetLayerWeight(2, 1);
        StartCoroutine(ResetAnimationTriggerAfterDelay("Fail", 0.15f)); // Ajusta el delay según sea necesario
    }

    private IEnumerator ResetAnimationTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        BoyFriendAnimator.ResetTrigger(triggerName);
        BoyFriendAnimator.SetLayerWeight(0, 1);
        BoyFriendAnimator.SetLayerWeight(2, 0);
    }


    private IEnumerator RestoreVoicesVolumeForShortDuration()
    {
        voices.mute = true;
        // Espera por un corto período de tiempo
        yield return new WaitForSeconds(0.05f);
        voices.mute = false;
        // Restaura el volumen de las voces
        AudioManager.Instance.RestoreVolumes();
    }

    public void ShowDeathScreen()
    {
        song.Stop();
        voices.Stop();
        BS.hasStarted = false;
        canvas.gameObject.SetActive(false);
        PlayerPrefs.SetString("CallingScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("DeathScreen");
    }
    public void ShowWinScreen()
    {
        // Detener la música y otros sonidos
        song.Stop();
        voices.Stop();
        BS.hasStarted = false;

        // Guardar los datos necesarios en PlayerPrefs
        PlayerPrefs.SetInt("Score", currentScore);
        PlayerPrefs.SetInt("Sicks", sicksCount);
        PlayerPrefs.SetInt("Goods", goodsCount);
        PlayerPrefs.SetInt("Misses", missesCount);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
        // Cambiar a la escena de victoria
        SceneManager.LoadScene("WinScreenScene");  // Asegúrate de que "WinScreen" es el nombre correcto de tu escena de victoria
    }


    private void ShowFeedbackImage(string trigger)
    {
        if (imagePool.Count > 0)
        {
            GameObject imageObject = GetImageObjectFromPool();
            if (imageObject != null)
            {
                // Obtener la referencia al contenedor de imágenes del combo
                Image[] comboImages = imageObject.GetComponentsInChildren<Image>();

                if (comboImages != null && comboImages.Length >= 3)
                {
                    // Modificar las imágenes del combo para representar el contador actual
                    int hundreds = comboCount / 100;
                    int tens = (comboCount / 10) % 10;
                    int ones = comboCount % 10;

                    comboImages[0].sprite = numberSprites[hundreds];
                    comboImages[1].sprite = numberSprites[tens];
                    comboImages[2].sprite = numberSprites[ones];

                    // Activar la animación
                    imageObject.GetComponent<Animator>().SetTrigger(trigger);
                    StartCoroutine(DespawnImageObject(imageObject));
                }
                else
                {
                    Debug.LogWarning("Combo images not found or incomplete.");
                }
            }
        }
        else
        {
            Debug.Log("Image pool is empty!");
        }
    }

    void UpdateHealthBar()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, 1);
        enemyHealth = Mathf.Clamp(enemyHealth, 0, 1);
        playerHealthBar.fillAmount = playerHealth;
        enemyHealthBar.fillAmount = enemyHealth;
        playerHealthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
        enemyHealthBar.fillOrigin = (int)Image.OriginHorizontal.Left;
        playerIcon.anchoredPosition = new Vector2(playerIconPosition, playerIcon.anchoredPosition.y);
        enemyIcon.anchoredPosition = new Vector2(-enemyIconPosition, enemyIcon.anchoredPosition.y);
        if (playerHealth == 0.0f)
        {
            ShowDeathScreen();
        }
    }

    private IEnumerator DespawnImageObject(GameObject imageObject)
    {
        yield return new WaitForSeconds(1f); // Adjust animation duration
        // Disable and return image object to pool
        imageObject.gameObject.SetActive(false);
        imagePool.Add(imageObject);
    }
    private IEnumerator Countdown()
    {
        // Reproduce los sonidos de cuenta atrás
        
        
        sfxSource.PlayOneShot(countdownSound3);
        yield return new WaitForSeconds(countdownSound3.length);
        sfxSource.PlayOneShot(countdownSound2);
        yield return new WaitForSeconds(countdownSound2.length);
        sfxSource.PlayOneShot(countdownSound1);
        yield return new WaitForSeconds(countdownSound1.length);
        sfxSource.PlayOneShot(introGo);
        yield return new WaitForSeconds(introGo.length);
        // Inicia la canción después de la cuenta atrás
        StartSong();
        BS.hasStarted = true;
        
    }
    public void changeCameraToEnemy()
    {
        // Cambia la cámara o inicia la transición
        cameraAnimator.SetTrigger("changeToEnemy");
    }
    public void changeCameraToPlayer()
    {
        // Cambia la cámara o inicia la transición
        cameraAnimator.SetTrigger("changeToPlayer");
    }



    private GameObject GetImageObjectFromPool()
    {
        if (imagePool.Count > 0)
        {
            // Remove the first object from the pool
            GameObject imageObject = imagePool[0];
            imagePool.RemoveAt(0);

            imageObject.transform.SetParent(contenedor.transform, true);

            // Activate the object
            imageObject.gameObject.SetActive(true);

            return imageObject;
        }
        else
        {
            // Handle case where pool is empty (e.g., print a message)
            Debug.Log("Image pool is empty!");
            return null;
        }
    }


}