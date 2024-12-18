using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength;
    [SerializeField] float rotationStrength;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

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
        Debug.Log("Colided with " + other.gameObject.tag);
        switch (other.gameObject.tag){
            case "GoldCoin_Collectible":
                other.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        applyThrust();
        applyRotation();
        applyBoosterEffects();
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
        rb.AddRelativeTorque(Vector3.forward * Time.fixedDeltaTime * rotation.ReadValue<float>() * rotationStrength);
    }

    void applyBoosterEffects(){
        if (thrust.IsPressed()){
            if (!mainBooster.isPlaying){
                mainBooster.Play();
            }
        } else{
            mainBooster.Stop();
        }

        if (rotation.ReadValue<float>() < 0){
            if (!rightBooster.isPlaying){
                rightBooster.Play();
            }
        } else {
            rightBooster.Stop();
        }
        
        if (rotation.ReadValue<float>() > 0){
            if (!leftBooster.isPlaying){
                leftBooster.Play();
            }
        } else {
            leftBooster.Stop();
        } 
    }
}
