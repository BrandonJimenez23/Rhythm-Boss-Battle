using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public bool hasStarted;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
        }
        else
        {
            transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
        }
    }
}
