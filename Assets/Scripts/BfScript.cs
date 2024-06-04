using UnityEngine;

public class BfScript : MonoBehaviour
{
    public Animator bfAnim;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;

    private float idleDelay = 0.5f; // Tiempo de espera antes de volver a la animación de idle
    private float idleTimer;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        bool isMoving = false;

        // Verifica cada tecla y activa el Trigger correspondiente
        if (Input.GetKey(leftKey))
        {
            bfAnim.SetTrigger("MoveLeft");
            isMoving = true;
        }
        if (Input.GetKey(rightKey))
        {
            bfAnim.SetTrigger("MoveRight");
            isMoving = true;
        }
        if (Input.GetKey(downKey))
        {
            bfAnim.SetTrigger("MoveDown");
            isMoving = true;
        }
        if (Input.GetKey(upKey))
        {
            bfAnim.SetTrigger("MoveUp");
            isMoving = true;
        }

        // Si no se presiona ninguna tecla, comenzar a contar el tiempo de inactividad
        if (!isMoving)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDelay)
            {
                ResetAllTriggers();
                bfAnim.SetBool("IsMoving", false);
                bfAnim.SetLayerWeight(1, 0); // Cambia el peso de la capa de movimiento a 0
            }
        }
        else
        {
            idleTimer = 0;
            bfAnim.SetBool("IsMoving", true);
            bfAnim.SetLayerWeight(1, 1); // Cambia el peso de la capa de movimiento a 1
        }
    }

    void ResetAllTriggers()
    {
        bfAnim.ResetTrigger("MoveLeft");
        bfAnim.ResetTrigger("MoveRight");
        bfAnim.ResetTrigger("MoveDown");
        bfAnim.ResetTrigger("MoveUp");
    }
}
