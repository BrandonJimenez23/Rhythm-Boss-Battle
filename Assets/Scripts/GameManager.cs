using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LongNoteObject[] longNotePrefab;
    public Section sectionPrefab;
    public OpponentLongNoteObject opponentLongNotePrefab;
    private DifficultySettings.Difficulty currentDifficulty;
    private (float healthGain, float healthLoss, float playerIconMoveGain, float playerIconMoveLoss, float enemyIconMoveGain, float enemyIconMoveLoss) difficultySettings;
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
    private List<WeekProgress> weekProgressList;
    // Nombres de archivo JSON para notas del jugador y del oponente
    public string playerNotesFile;
    public string opponentNotesFile;
    public Button backButton;
    private float songTimeCounter;
    void Start()
    {
        SetDifficultySettings();
        InitializeGame();
        
    }

    void Update()
    {
        UpdateUI();
        // Incrementar el contador de tiempo de la canción solo si está sonando
        if (song.isPlaying)
        {
            songTimeCounter += Time.deltaTime;
        }

        // Verificar si la canción ha terminado con un pequeño margen de error
        if (songTimeCounter >= song.clip.length - 0.5f) // Puedes ajustar el margen según sea necesario
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
    void SetDifficultySettings()
    {
        GameProgress gameProgress = ProgressManager.instance.gameProgress;
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        weekProgressList = gameProgress.weeks;
        foreach (var week in weekProgressList)
        {
            for (int i = 0; i< week.levels.Count; i++)
            {
                if (week.levels[i].levelName == currentLevel)
                {
                    
                    if (System.Enum.TryParse(week.tracks[i].difficulty, out currentDifficulty))
                    {
                        difficultySettings = DifficultySettings.GetSettings(currentDifficulty);
                    }
                    else
                    {
                        Debug.LogError("Invalid difficulty setting: " + week.tracks[i].difficulty);
                    }
                }
            }

        }
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
        int sectionSize = 50;
        int noteCount = 0;
        Section currentSection = null;

        foreach (Note note in playerNotes)
        {
            if (noteCount % sectionSize == 0)
            {
                // Crear una nueva sección
                currentSection = Instantiate(sectionPrefab,  BS.transform);
                currentSection.activationTime = note.Time;
                // Ajustar la posición de la sección a la misma altura que la primera nota
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                currentSection.transform.position = new Vector3(currentSection.transform.position.x, initialPositionY, currentSection.transform.position.z);
            }

            if (note.Key >= 0 && note.Key < notePrefabs.Length)
            {
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                Vector3 spawnPosition = new Vector3(noteSpawnPoints[note.Key].position.x, initialPositionY, noteSpawnPoints[note.Key].position.z);

                if (note.Duration != 0)
                {
                    LongNoteObject longNoteObject = Instantiate(longNotePrefab[note.Key], spawnPosition, Quaternion.identity, currentSection.transform);
                    longNoteObject.time = adjustedTime;
                    longNoteObject.fallSpeed = BS.speed;
                    longNoteObject.duration = note.Duration;
                    longNoteObject.key = note.Key;

                    longNoteObject.arrow.Initialize(note.Key, adjustedTime);
                    longNoteObject.tail.Initialize(note.Duration, BS.speed, note.Key);
                    longNoteObject.gameObject.SetActive(false);
                }
                else
                {
                    NoteObject noteObject = Instantiate(notePrefabs[note.Key], spawnPosition, Quaternion.identity, currentSection.transform);
                    noteObject.key = note.Key;
                    noteObject.time = adjustedTime;
                    noteObject.offset = countdownSound1.length + countdownSound2.length + countdownSound3.length + introGo.length;
                    noteObject.gameObject.SetActive(false);
                }
                noteCount++;
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
        int sectionSize = 100;
        int noteCount = 0;
        Section currentSection = null;

        foreach (Note note in opponentNotes)
        {
            if (noteCount % sectionSize == 0)
            {
                // Crear una nueva sección
                currentSection = Instantiate(sectionPrefab, BS.transform);
                currentSection.activationTime = note.Time;
                // Ajustar la posición de la sección a la misma altura que la primera nota
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                currentSection.transform.position = new Vector3(currentSection.transform.position.x, initialPositionY, currentSection.transform.position.z);
            }

            if (note.Key >= 0 && note.Key < notePrefabs.Length)
            {
                float adjustedTime = note.Time;
                float initialPositionY = BS.speed * adjustedTime;
                Vector3 spawnPosition = new Vector3(enemySpawnPoints[note.Key].position.x, initialPositionY, enemySpawnPoints[note.Key].position.z);

                if (note.Duration != 0)
                {
                    OpponentLongNoteObject longNoteObject = Instantiate(opponentLongNotePrefab, spawnPosition, Quaternion.identity, currentSection.transform);
                    longNoteObject.fallSpeed = BS.speed;
                    longNoteObject.duration = note.Duration;
                    longNoteObject.key = note.Key;

                    // Asignar referencias de arrow y tail antes de inicializarlas
                    longNoteObject.arrow = longNoteObject.GetComponentInChildren<OpponentArrowNotePart>();
                    longNoteObject.tail = longNoteObject.GetComponentInChildren<OpponentTailNotePart>();

                    longNoteObject.arrow.Initialize(note.Key, BS.speed);
                    longNoteObject.tail.Initialize(note.Duration, BS.speed, note.Key);
                    longNoteObject.gameObject.SetActive(false);
                }
                else
                {
                    OpponentNoteObject noteObject = Instantiate(opponentNotePrefab, spawnPosition, Quaternion.identity, currentSection.transform);
                    noteObject.SetSprite(note.Key);
                    noteObject.gameObject.SetActive(false);
                }
                noteCount++;
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
        songTimeCounter = 0f;
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
        sicksCount++;
        AdjustHealthAndIconPositions(difficultySettings.healthGain, difficultySettings.playerIconMoveGain, difficultySettings.enemyIconMoveGain);
        ShowFeedbackImage("ShowPerfect");
        particleController.PlayParticles(buttonIndex);
    }

    public void HitNoteSick()
    {
        currentScore += 50;
        comboCount++;
        AdjustHealthAndIconPositions(difficultySettings.healthGain, difficultySettings.playerIconMoveGain, difficultySettings.enemyIconMoveGain);
    }

    public void HitNoteGood()
    {
        currentScore += 25;
        goodsCount++;
        comboCount++;
        AdjustHealthAndIconPositions(difficultySettings.healthGain, difficultySettings.playerIconMoveGain, difficultySettings.enemyIconMoveGain);
        ShowFeedbackImage("ShowGood");
    }

    public void MissNote()
    {
        currentScore = Mathf.Max(currentScore - 10, 0);
        comboCount = 0;
        missesCount++;
        AdjustHealthAndIconPositions(-difficultySettings.healthLoss, difficultySettings.playerIconMoveLoss, difficultySettings.enemyIconMoveLoss);
        ShowFeedbackImage("ShowBad");
        HandleFail();
    }

    private void AdjustHealthAndIconPositions(float healthAdjustment, float playerIconMove, float enemyIconMove)
    {
        playerHealth = Mathf.Clamp(playerHealth + healthAdjustment, 0, 1);
        enemyHealth = Mathf.Clamp(enemyHealth - healthAdjustment, 0, 1); // Ajusta esto según la lógica de tu juego
        playerIconPosition = Mathf.Clamp(playerIconPosition + playerIconMove, -42, 50);
        enemyIconPosition = Mathf.Clamp(enemyIconPosition + enemyIconMove, -42, 50);
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
                // Obtener la referencia al componente ComboImageReferences
                ComboImageReferences comboRefs = imageObject.GetComponent<ComboImageReferences>();

                if (comboRefs != null)
                {
                    // Modificar las imágenes del combo para representar el contador actual
                    int hundreds = comboCount / 100;
                    int tens = (comboCount / 10) % 10;
                    int ones = comboCount % 10;

                    comboRefs.hundredsImage.sprite = numberSprites[hundreds];
                    comboRefs.tensImage.sprite = numberSprites[tens];
                    comboRefs.onesImage.sprite = numberSprites[ones];

                    // Activar la animación
                    imageObject.GetComponent<Animator>().SetTrigger(trigger);
                    StartCoroutine(DespawnImageObject(imageObject));
                }
                else
                {
                    Debug.LogWarning("ComboImageReferences component not found.");
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
