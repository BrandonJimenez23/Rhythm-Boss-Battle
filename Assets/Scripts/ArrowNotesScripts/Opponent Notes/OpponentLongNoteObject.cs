using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentLongNoteObject : MonoBehaviour
{
    public HexScript hexScript;
    public Image arrowImage; // Image component for the arrow part of the note
    public Image tailImage; // Image component for the tail part
    public Sprite[] arrowSprites; // Sprites for different arrow keys
    public Sprite[] tailSprite; // Sprite for the tail part

    public OpponentArrowNotePart arrow;
    public OpponentTailNotePart tail;

    [SerializeField]
    public float fallSpeed;
    [SerializeField]
    public int key;
    [SerializeField]
    public float duration;

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

        // Obtener las referencias de las partes de la nota larga
        arrow = GetComponentInChildren<OpponentArrowNotePart>();
        tail = GetComponentInChildren<OpponentTailNotePart>();

        // Inicializar las partes de la nota larga
        arrow.Initialize(key, fallSpeed);
        tail.Initialize(duration, fallSpeed, key);

        // Configurar las imágenes
        arrowImage.sprite = arrowSprites[key];
        tailImage.sprite = tailSprite[key];
    }

    void Update()
    {
        // Manejar la interacción entre la cabeza y la cola de la nota larga
        if (arrow.isPressedCorrectly)
        {
            tail.canBePressed = true; // Esto asegura que la cola comienza a reducirse
        }
    }
}
