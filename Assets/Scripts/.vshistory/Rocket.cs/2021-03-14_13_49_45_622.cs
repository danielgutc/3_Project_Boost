using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    bool isTransitioning = false;

    [SerializeField] float rcsThrust = 50f;
    [SerializeField] float mainThrust = 50f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            Thrust();
            Rotate();
        }
        
    }


    #region Rocket

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                //StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    void StartDeathSequence()
    {
        Invoke("LoadFirstLevel", 2f);

    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void Thrust(bool breaking = false)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime * (breaking? -1: 1));
            mainEngineParticles.Play();

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSound);
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ManualRotate(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ManualRotate(-rcsThrust * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            Thrust(true);
        }
    }

    private void ManualRotate(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;
    }
    #endregion

    #region Levels

    #endregion
}
