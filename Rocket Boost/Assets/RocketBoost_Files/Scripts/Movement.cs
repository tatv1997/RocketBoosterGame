using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, tpically set in the editor
    
    // CACHE - eg. references for readability and speed 

    // STATE - private instances (member) variables
    
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 10f;
    [SerializeField] float rotationStrength = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainboosterParticles;
    [SerializeField] ParticleSystem leftboosterParticles;
    [SerializeField] ParticleSystem rightboosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    private void Start()
    {
       rb =  GetComponent<Rigidbody>();
       audioSource = GetComponent<AudioSource>();
    }


    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        if (thrust.IsPressed())
        {
            ProcessThrust();
        }
        else
        {
            StopThrust();
        }
        ProcessRotation();
    }

    public void ProcessThrust()
    {
        
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainboosterParticles.isPlaying)
        {
            mainboosterParticles.Play();
        }
    }

    public void StopThrust()
    {
        audioSource.Stop();
        mainboosterParticles.Stop();
    }


    public void RotateRocketRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightboosterParticles.isPlaying)
        {
            leftboosterParticles.Stop();
            rightboosterParticles.Play();
        }

    }

    public void RotateRocketLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftboosterParticles.isPlaying)
        {
            rightboosterParticles.Stop();
            leftboosterParticles.Play();
        }
    }


    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput > 0)
        {
            RotateRocketRight();

        }

        else if (rotationInput < 0)
        {
            RotateRocketLeft();
        }
        else 
        {
            rightboosterParticles.Stop();
            leftboosterParticles.Stop();
        }

    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}



