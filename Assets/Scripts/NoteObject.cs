using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Note note; // Añadimos esta línea
    public bool canBePressed;
    private float noteActivationTime;
    
    private float duration;
    public KeyCode keyToPress;
    [SerializeField]
    public int key;

    [SerializeField]
    public float time;

    [SerializeField]
    public float offset;

    private AudioSource song;
    void Start()
    {
        // Obtener la referencia al AudioSource desde el GameManager
        song = FindObjectOfType<GameManager>().song;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                float timeDifference = song.time-(time-0.15f);
                string precision = CalculatePrecision(timeDifference);
                Debug.Log("Time difference (ms): " + (timeDifference * 1000f));

                switch (precision)
                {
                    case "Perfect":
                        FindObjectOfType<GameManager>().HitNotePerfect(GetKeyIndex(keyToPress));
                        break;
                    case "Sick":
                        FindObjectOfType<GameManager>().HitNoteSick();
                        break;
                    case "Good":
                        FindObjectOfType<GameManager>().HitNoteGood();
                        break;
                    default:
                        FindObjectOfType<GameManager>().MissNote();
                        break;
                }
            }
        }
    }

    private string CalculatePrecision(float timeDifference)
    {
        float perfectThreshold = 0.100f;
        float goodThreshold = 0.200f;

        if (Mathf.Abs(timeDifference) <= perfectThreshold)
        {
            return "Perfect";
        }
        else if (Mathf.Abs(timeDifference) <= goodThreshold)
        {
            return "Good";
        }
        else
        {
            return "Miss";
        }
    }
    public void CalculatePressTime(float fallSpeed)
    {
        if (time <= 0) // Si el tiempo no está definido o es 0, calcularlo
        {
            float height = transform.position.y; // Obtener la altura de la nota
            time = height / fallSpeed; // Calcular el tiempo de pulsación
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MissBarrier")
        {
            Debug.Log("Barrier Enter!");
            FindObjectOfType<GameManager>().MissNote();
            gameObject.SetActive(false);
        }
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;
        }
    }
    public int GetKeyIndex(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A:
                return 0;
            case KeyCode.S:
                return 1;
            case KeyCode.K:
                return 2;
            case KeyCode.L:
                return 3;
            default:
                return -1; // Devuelve -1 para teclas no mapeadas
        }
    }

}
