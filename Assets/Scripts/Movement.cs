using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength;
    [SerializeField] float rotationStrength;

    private AudioSource audioSource;
    Rigidbody rb;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag){
            case "Fuel":
                other.gameObject.SetActive(false);
                break;
        }
    }

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
                NextLevel();
                break;
            default:
                RepeatLevel();
                break;
        }
    }

    void NextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level += 1;

        SceneManager.LoadScene(level);
    }

    void RepeatLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        applyThrust();
        applyRotation();
    }

    void applyThrust()
    {
        if (thrust.IsPressed()){
            if (!audioSource.isPlaying){
                audioSource.Play();
            }
            rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * thrustStrength);
        } else {
            audioSource.Stop();
        }
    }

    void applyRotation()
    {
        rb.AddRelativeTorque(Vector3.right * Time.fixedDeltaTime * rotation.ReadValue<float>() * rotationStrength);
    }
}
