using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0, 1)][SerializeField] float moveFactor;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycle = Time.time / period;
        const float tau = 2f * Mathf.PI;
        float sinWave = Mathf.Sin(cycle * tau);
        moveFactor = sinWave / 2f + 0.5f;

        Vector3 offset = movementVector * moveFactor;
        transform.position = startPos + offset;
    }
}
