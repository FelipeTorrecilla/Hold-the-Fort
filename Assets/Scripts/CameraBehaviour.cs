using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Camera movement smoothness

    private Vector3 offset; // Offset between the camera and the player

    void Start()
    {
        offset = transform.position - target.position; // Calculate the initial offset
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // Calculate the desired camera position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smoothly move the camera towards the desired position
        transform.position = smoothedPosition; // Update the camera's position
    }
}