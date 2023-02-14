using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidBody;
    AudioSource thrust;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelEnd;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathSoundParticles;
    [SerializeField] ParticleSystem levelEndParticles;

    [SerializeField] bool canCollide = false;

    int index = 0;

    enum State
    {
        Alive, Dying, Transcending
    };
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thrust = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            CheckThrust();

            CheckRotation();

            LoadNext();

            ToggleCollision();

        }

    }

    void LoadNext()
    {

        if (Input.GetKey(KeyCode.L))
        {
            LoadScene();
        }

    }

    void ToggleCollision()
    {

        if (Input.GetKey(KeyCode.C))
        {
            canCollide = !canCollide;
        }
    }

    void CheckThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            ApplyThrust();
        }

        else
        {
            thrust.Stop();
            mainEngineParticles.Stop();
        }
    }

    void ApplyThrust()
    {
        mainEngineParticles.Play();

        rigidBody.AddRelativeForce(Vector3.up * mainThrust);

        if (!thrust.isPlaying)
        {
            thrust.PlayOneShot(mainEngine);
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

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !canCollide)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {

            case "Friendly":
                break;

            case "Finish":
                SuccessSequence();
                break;

            default:
                DeathSequence();
                break;
        }
    }

    private void SuccessSequence()
    {
        state = State.Transcending;
        index++;
        thrust.Stop();
        thrust.PlayOneShot(levelEnd);
        levelEndParticles.Play();
        Invoke("LoadScene", 1f);
    }

    private void DeathSequence()
    {
        state = State.Dying;
        index = 0;
        thrust.Stop();
        thrust.PlayOneShot(deathSound);
        deathSoundParticles.Play();
        Invoke("LoadScene", 1f);
    }

    private void LoadScene()
    {
        deathSoundParticles.Stop();
        SceneManager.LoadScene(index);
    }
}
