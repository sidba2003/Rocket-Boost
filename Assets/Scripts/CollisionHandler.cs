using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] int methodCallDelay;

    void OnCollisionEnter(Collision other){
        switch (other.gameObject.tag){
            case "Start": 
                Debug.Log("Currently at start"); 
                break;
            case "Fuel": // this will most probably never be executed
                other.gameObject.SetActive(false);
                Debug.Log("Collected Fuel");
                break;
            case "Finish":
                Debug.Log("You have finished this level!!!");
                StartSequence("NextLevel");
                break;
            default:
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
    }

    void RepeatLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}
