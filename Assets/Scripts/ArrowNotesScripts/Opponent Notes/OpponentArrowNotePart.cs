using UnityEngine;

public class OpponentArrowNotePart : MonoBehaviour
{
    [SerializeField]
    public int key;
    public bool canBePressed;
    public bool isPressedCorrectly;
    public float fallSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get animation method based on collider's tag
        string arrowTag = other.tag; // Assuming "Activator" has a suffix like "LeftActivator"

        switch (arrowTag)
        {
            case "LeftActivator":
                isPressedCorrectly = true;
                break;
            case "RightActivator":
                isPressedCorrectly = true;
                break;
            case "UpActivator":
                isPressedCorrectly = true;
                break;
            case "DownActivator":
                isPressedCorrectly = true;
                break;
            default:
                Debug.LogError("Unsupported activator tag: " + arrowTag);
                break;
        }

        // Deactivate the note object after triggering animation
        gameObject.SetActive(false);
    }
    public void Initialize(int key, float fallSpeed)
    {
        this.key = key;
        this.fallSpeed = fallSpeed;
    }
}
