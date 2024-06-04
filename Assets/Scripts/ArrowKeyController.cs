using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import UnityEngine.UI for Image component

public class ArrowKeyController : MonoBehaviour
{
    // Start is called before the first frame update
    private Image currentImage;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    void Start()
    {
        currentImage = GetComponent<Image>(); // Get the Image component

        // Ensure default and pressed images are assigned
        if (!defaultImage || !pressedImage)
        {
            Debug.LogError("Please assign default and pressed images in the inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            currentImage.sprite = pressedImage; // Use sprite from pressedImage
        }

        if (Input.GetKeyUp(keyToPress))
        {
            currentImage.sprite = defaultImage; // Use sprite from defaultImage
        }
    }
}
