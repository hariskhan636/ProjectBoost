using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidBody;
    AudioSource thrust;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thrust = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckThrust();

        CheckRotation();
    }
    void CheckThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            if (!thrust.isPlaying)
            {
                thrust.Play();
            }

        }
        else
        {
            thrust.Stop();
        }
    }
    void CheckRotation()
    {

        rigidBody.freezeRotation = true;

        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

}
