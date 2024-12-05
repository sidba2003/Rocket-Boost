using UnityEngine;
using UnityEngine.InputSystem;

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
