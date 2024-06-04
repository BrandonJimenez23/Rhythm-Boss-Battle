using UnityEngine;

public class LongNoteObject : MonoBehaviour
{
    public ArrowNotePart arrow;
    public TailNotePart tail;
    [SerializeField]
    public float fallSpeed;
    [SerializeField]
    public int key;
    [SerializeField]
    public float time;
    [SerializeField]
    public float duration;
    
    void Start()
    {

    }

    void Update()
    {

        if (arrow.isPressedCorrectly && tail.canBePressed)
        {
            tail.canBePressed = true; // Esto asegura que la cola comienza a reducirse
        }
    }
}
