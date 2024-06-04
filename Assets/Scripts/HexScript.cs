using UnityEngine;

public class HexScript : MonoBehaviour
{
    public Animator hexAnim;
    private string currentAnimation = "";
    private float idleDelay = 0.5f; // Tiempo de espera antes de volver a la animación de idle
    private float idleTimer;

    void Update()
    {
        // Si no se está reproduciendo ninguna animación, comenzar a contar el tiempo de inactividad
        if (currentAnimation == "")
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDelay)
            {
                hexAnim.SetBool("IsMoving", false);
                hexAnim.SetLayerWeight(1, 0); // Cambia el peso de la capa de movimiento a 0
            }
        }
        else
        {
            // Si la animación actual ha terminado, reinicia currentAnimation
            if (!hexAnim.GetCurrentAnimatorStateInfo(1).IsName(currentAnimation))
            {
                currentAnimation = "";
            }
            idleTimer = 0;
        }
    }

    public void PlayAnimation(string animation)
    {
        ResetAllTriggers();
        hexAnim.SetTrigger(animation);
        currentAnimation = animation;
        hexAnim.SetBool("IsMoving", true);
        hexAnim.SetLayerWeight(1, 1); // Cambia el peso de la capa de movimiento a 1
    }

    void ResetAllTriggers()
    {
        hexAnim.ResetTrigger("isLeftPressed");
        hexAnim.ResetTrigger("isRightPressed");
        hexAnim.ResetTrigger("isUpPressed");
        hexAnim.ResetTrigger("isDownPressed");
    }
}
