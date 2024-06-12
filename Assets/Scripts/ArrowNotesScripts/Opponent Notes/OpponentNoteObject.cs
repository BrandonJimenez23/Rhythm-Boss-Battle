using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import UnityEngine.UI for Image component

public class OpponentNoteObject : MonoBehaviour
{
    public HexScript hexScript;
    public Image noteImage; // Reference to the Image component of the note object
    public Sprite[] arrowSprites;

    void Start()
    {
        GameObject hexObject = GameObject.Find("Hexx");
        if (hexObject != null)
        {
            hexScript = hexObject.GetComponent<HexScript>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto con el script HexScript");
        }

        noteImage = GetComponent<Image>(); // Get the Image component
    }

    public void SetSprite(int key)
    {
        // Ensure key is within valid range
        if (key >= 0 && key < arrowSprites.Length)
        {
            noteImage.sprite = arrowSprites[key]; // Set sprite using Image component
        }
        else
        {
            Debug.LogError("Invalid key: " + key);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get animation method based on collider's tag
        string arrowTag = other.tag; // Assuming "Activator" has a suffix like "LeftActivator"

        switch (arrowTag)
        {
            case "LeftActivator":
                hexScript.PlayAnimation("isLeftPressed");
                break;
            case "RightActivator":
                hexScript.PlayAnimation("isRightPressed");
                break;
            case "UpActivator":
                hexScript.PlayAnimation("isUpPressed");
                break;
            case "DownActivator":
                hexScript.PlayAnimation("isDownPressed");
                break;
            default:
                Debug.LogError("Unsupported activator tag: " + arrowTag);
                break;
        }

        // Deactivate the note object after triggering animation
        gameObject.SetActive(false);
    }
}