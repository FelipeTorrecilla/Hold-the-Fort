using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Camera movement smoothness

    private Vector3 offset; // Offset between the camera and the player
    private bool isShaking = false; // Flag to check if camera shake is active
    private float shakeDuration = 0f; // Duration of the camera shake
    private float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    void Start()
    {
        offset = transform.position - target.position; // Calculate the initial offset
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // Calculate the desired camera position

        // Apply camera shake effect if active
        if (isShaking)
        {
            desiredPosition += Random.insideUnitSphere * shakeMagnitude;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smoothly move the camera towards the desired position
        transform.position = smoothedPosition; // Update the camera's position
    }
    
    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        isShaking = true;
        Invoke(nameof(StopShaking), duration);
    }
    private void StopShaking()
    {
        isShaking = false;
    }
    
}