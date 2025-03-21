using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing speed for camera movement
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f); // Offset of the camera relative to the slime

    void FixedUpdate()
    {
        // Access the furthest slime directly
        if (SlimeMovement.furthestSlime != null)
        {
            Vector3 desiredPosition = SlimeMovement.furthestSlime.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}