using UnityEngine;
using UnityEngine.UI;

public class OpponentTailNotePart : MonoBehaviour
{
    public bool canBePressed;
    [SerializeField]
    public float noteDuration;
    private float remainingDuration;
    private float initialSize = 44.0f; // Tamaño inicial de la nota en el eje Y a escala 1
    [SerializeField]
    public float fallSpeed;
    public Image progressBar; // Barra de progreso UI
    public HexScript hexScript;
    public int key;

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
        if (canBePressed)
        {
            if (remainingDuration == noteDuration)
            {
                PlayAnimationForKey(); // Activar la animación al principio
            }

            remainingDuration -= Time.deltaTime;
            if (progressBar != null)
            {
                progressBar.fillAmount = remainingDuration / noteDuration; // Actualiza la barra de progreso
                PlayAnimationForKey();
            }

            if (remainingDuration <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Initialize(float noteDuration, float fallSpeed, int key)
    {
        this.noteDuration = noteDuration;
        this.fallSpeed = fallSpeed;
        this.key = key;
    }

    private void PlayAnimationForKey()
    {
        // Activar la animación correspondiente en HexScript
        switch (key)
        {
            case 0:
                hexScript.PlayAnimation("isLeftPressed");
                break;
            case 1:
                hexScript.PlayAnimation("isRightPressed");
                break;
            case 2:
                hexScript.PlayAnimation("isUpPressed");
                break;
            case 3:
                hexScript.PlayAnimation("isDownPressed");
                break;
            default:
                Debug.LogError("Invalid key: " + key);
                break;
        }
    }

}
