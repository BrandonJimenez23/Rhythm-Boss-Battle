using UnityEngine;
using UnityEngine.UI;

public class TailNotePart : MonoBehaviour
{
    public KeyCode keyToPress;
    public bool canBePressed;
    [SerializeField]
    public float noteDuration;
    private float remainingDuration;
    private float initialSize = 44.0f; // Tamaño inicial de la nota en el eje Y a escala 1
    [SerializeField]
    public float fallSpeed;
    public Image progressBar; // Barra de progreso UI
    private int score = 0; // Puntaje acumulado
    private float scoreTimer = 0.0f; // Temporizador para acumular el puntaje

    void Start()
    {
        remainingDuration = noteDuration;
        if (progressBar != null)
        {
            progressBar.fillAmount = 1.0f; // Inicialmente llena
        }
        // Calcula la escala inicial en y
        float initialScaleY = (fallSpeed * noteDuration) / initialSize;

        // Ajusta la escala inicial del objeto
        transform.localScale = new Vector3(transform.localScale.x, initialScaleY, transform.localScale.z);
    }

    void Update()
    {
        if (Input.GetKey(keyToPress) && canBePressed)
        {
            remainingDuration -= Time.deltaTime;
            scoreTimer += Time.deltaTime; // Incrementa el temporizador

            if (scoreTimer >= 0.25f) // Cada segundo
            {
                score += 50; // Aumenta el puntaje en 50
                scoreTimer -= 0.25f; // Resetea el temporizador para el siguiente segundo
            }

            if (progressBar != null)
            {
                progressBar.fillAmount = remainingDuration / noteDuration; // Actualiza la barra de progreso
            }

            if (remainingDuration <= 0)
            {
                FindObjectOfType<GameManager>().LongNoteScore(score); // Agrega el puntaje acumulado al puntaje total
                gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyUp(keyToPress) && canBePressed)
        {
            FindObjectOfType<GameManager>().LongNoteScore(score); // Agrega el puntaje acumulado al puntaje total
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    public void Initialize(float noteDuration, float fallSpeed, int key)
    {
        this.noteDuration = noteDuration;
        this.fallSpeed = fallSpeed;
        this.keyToPress = GetKeyCodeForKey(key); // Asigna el KeyCode correspondiente
    }

    private KeyCode GetKeyCodeForKey(int key)
    {
        // Retorna el KeyCode basado en el valor de 'key'
        switch (key)
        {
            case 0: return KeyCode.A;
            case 1: return KeyCode.S;
            case 2: return KeyCode.K;
            case 3: return KeyCode.L;
            // Agrega más casos según sea necesario
            default: return KeyCode.None;
        }
    }
}
