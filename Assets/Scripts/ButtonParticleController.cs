using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem[] particleSystems; // Array de sistemas de partículas
    public Transform particleContainer; // El objeto contenedor de partículas

    void Start()
    {

    }

    public void PlayParticles(int particleIndex)
    {
        if (particleIndex >= 0 && particleIndex < particleSystems.Length)
        {
            if (particleSystems[particleIndex] != null)
            {
                particleSystems[particleIndex].Play();
            }
            else
            {
                Debug.LogWarning($"Particle system at index {particleIndex} is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid particle index for particle emission.");
        }
    }

}
