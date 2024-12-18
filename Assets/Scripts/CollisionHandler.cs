using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] int methodCallDelay;

    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    private AudioSource audioSource;
    private bool isControllable = true;
    private bool isCollisionsOn = true;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        checkDebugKeys();
    }

    void checkDebugKeys(){
        if (Keyboard.current.lKey.wasPressedThisFrame){
            NextLevel();
        } 
        else if (Keyboard.current.cKey.wasPressedThisFrame){
            isCollisionsOn = !isCollisionsOn;
        }
    }

    void OnCollisionEnter(Collision other){
        if (!isControllable || !isCollisionsOn){
            return;
        }

        switch (other.gameObject.tag){
            case "Start":  
                break;
            case "jumppad":
                break;
            case "Finish":
                isControllable = false;
                audioSource.PlayOneShot(successSound);
                successParticles.Play();
                StartSequence("NextLevel");
                break;
            default:
                isControllable = false;
                audioSource.PlayOneShot(crashSound);
                crashParticles.Play();
                StartSequence("RepeatLevel");
                break;
        }
    }

    void StartSequence(String CallMethod){
        GetComponent<Movement>().enabled = false; // disable the movement script, so the player can't move after crashing
        Invoke(CallMethod, methodCallDelay);
    }

    void NextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level += 1;

        if (level == SceneManager.sceneCountInBuildSettings){
            level = 0;
        }

        SceneManager.LoadScene(level);
        isControllable = true;
    }

    void RepeatLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
        isControllable = true;
    }
}
