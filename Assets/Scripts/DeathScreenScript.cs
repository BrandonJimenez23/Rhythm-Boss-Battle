using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator component

    private bool animationPlaying = false; // Flag to track animation state
    public AudioClip deathSound;
    public AudioClip endOfDeathScreen;
    public AudioSource sfxSource;
    public AudioSource song;

    void Start()
    {
        // Assuming the animator has a trigger named "isPlayerDefeated"
        playerAnimator.SetTrigger("isPlayerDefeated");
        StartCoroutine(DeathSounds());
    }

    void Update()
    {
        // Continuously check for Enter key press in Update
        if (Input.GetKeyDown(KeyCode.Return) && !animationPlaying)
        {
            // Start the EnterPressed animation
            playerAnimator.SetTrigger("playerTryingAgain");
            animationPlaying = true;
            OnEnterPressed();
        }
    }

    // This method should be called from an animation event at the end of the EnterPressed animation
    public void AnimationFinished() // Animation event callback function
    {
        // Reset animation flag
        animationPlaying = false;

        // Call the scene loading method after animation finishes
        OnEnterPressed();
    }
    private IEnumerator DeathSounds()
    {
        // Reproduce los sonidos de cuenta atrás
        sfxSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(deathSound.length);
        song.Play();

    }
    public void OnEnterPressed()
    {
        // Get the name of the scene that called this death screen (passed as a parameter)
        sfxSource.PlayOneShot(endOfDeathScreen);
        song.Stop();
        string callingSceneName = PlayerPrefs.GetString("CallingScene");

        // Start a coroutine to wait for the animation to finish before loading the scene
        StartCoroutine(LoadNextSceneAfterAnimation(callingSceneName));
    }

    private IEnumerator LoadNextSceneAfterAnimation(string callingSceneName)
    {
        // Yield until the animation is finished
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        AudioManager.Instance.RestoreVolumes();

        // Load the calling scene or your main scene if no scene name is saved
        SceneManager.LoadScene(callingSceneName.Length > 0 ? callingSceneName : "Main"); // Replace "MainScene" with your actual scene name
    }
}
