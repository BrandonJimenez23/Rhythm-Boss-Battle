using UnityEngine;

public class ArrowNotePart : MonoBehaviour
{
    public KeyCode keyToPress;
    [SerializeField]
    public int key;
    [SerializeField]
    public float time;
    public bool canBePressed;
    public bool isPressedCorrectly;
    private AudioSource song;
    private void Start()
    {
        song = FindObjectOfType<GameManager>().song;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && canBePressed)
        {
            isPressedCorrectly = true;
            gameObject.SetActive(false);
            float timeDifference = song.time - (time - 0.150f);
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
    public void Initialize(int key, float time)
    {
        this.key = key;
        this.time = time;
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
