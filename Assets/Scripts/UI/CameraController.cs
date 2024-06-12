using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform player;
    public Transform enemy;
    public Vector3 beatScale = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 originalScale;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Aquí puedes cambiar el turno basado en tu lógica de notas
    }

    public void OnBeat()
    {
        StartCoroutine(BeatAnimation());
    }

    private IEnumerator BeatAnimation()
    {
        transform.localScale = beatScale;
        yield return new WaitForSeconds(0.1f); // Duración del latido
        transform.localScale = originalScale;
    }

    public void SwitchFocus(bool toPlayer)
    {
        if (toPlayer)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(enemy.position.x, enemy.position.y, transform.position.z);
        }
    }
}
