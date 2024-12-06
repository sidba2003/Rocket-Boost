using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Audio;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] int methodCallDelay;

    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    private AudioSource audioSource;
    private bool isControllable = true;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other){
        if (!isControllable){
            return;
        }

        switch (other.gameObject.tag){
            case "Start":  
                break;
            case "Fuel": // this will never be executed
                other.gameObject.SetActive(false);
                break;
            case "Finish":
                isControllable = false;
                audioSource.PlayOneShot(successSound);
                StartSequence("NextLevel");
                break;
            default:
                isControllable = false;
                audioSource.PlayOneShot(crashSound);
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
