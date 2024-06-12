using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowKeyController : MonoBehaviour
{
    private Image currentImage;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public KeyCode keyToPress;
    private bool noteInArea; // Indicates if a note is in the detection area

    void Start()
    {
        currentImage = GetComponent<Image>();

        if (!defaultImage || !pressedImage)
        {
            Debug.LogError("Please assign default and pressed images in the inspector!");
        }

        noteInArea = false; // Initially no note is in the detection area
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            currentImage.sprite = pressedImage;

            if (noteInArea)
            {
                // Logic for hitting a note
                Debug.Log("Note hit!");
            }
            else
            {
                // Penalize the player for ghost tapping
                FindObjectOfType<GameManager>().MissNote();// Assuming GameManager is a singleton
                Debug.Log("Ghost tap! Missed!");
            }
        }

        if (Input.GetKeyUp(keyToPress))
        {
            currentImage.sprite = defaultImage;
        }
    }

    // Use OnTriggerEnter2D to detect when a note enters the area
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerNote"))
        {
            noteInArea = true;
        }
    }

    // Use OnTriggerExit2D to detect when a note exits the area
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerNote"))
        {
            noteInArea = false;
        }
    }
}
