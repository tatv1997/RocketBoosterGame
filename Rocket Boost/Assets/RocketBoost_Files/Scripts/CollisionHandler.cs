using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f; // Delay before loading the next level
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] InputAction nextlevelkey;


    AudioSource audioSource;
    

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnEnable()
    {
        nextlevelkey.Enable();
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
            Debug.Log("L key pressed - Reloading level");
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            Debug.Log("C key was pressed");
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !isCollidable) { return; }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly collision detected.");
                break;

            case "Finish":
                StartSuccessSequence();
                Debug.Log("Finish Line Reached! Go to next level");
                break;


            default:
                StartCrashSequence();
                Debug.Log("You crashed! Reloading level...");
                break;

        }
    }

    void StartSuccessSequence()
    {
        // TODO: Add SFX and particles
        isControllable = false; // Prevent further collisions
        audioSource.Stop(); // Stop any ongoing engine sound
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        // TODO: Add SFX and particles
        isControllable = false; // Prevent further collisions
        audioSource.Stop(); // Stop any ongoing engine sound
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay); 
    
    }

    void LoadNextLevel() 
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
 
    }
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

}
