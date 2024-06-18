using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // The point to orbit around
    public float distance = 10.0f; // Distance from the target
    public float heightOffset = 5.0f; // Height above the target
    public float orbitSpeed = 10.0f; // Speed of orbiting
    public float downwardAngle = 30.0f; // Downward angle of the camera

    private float currentAngle = 0.0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not set for CameraOrbit script.");
            return;
        }

        // Calculate the initial position
        UpdatePosition();
    }

    void Update()
    {
        if (target == null) return;

        // Update the angle based on time and speed
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360; // Keep the angle within 0-360 degrees

        // Update the camera position
        UpdatePosition();
    }

    void UpdatePosition()
    {
        // Calculate the new position based on the angle
        float radianAngle = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radianAngle) * distance;
        float z = Mathf.Cos(radianAngle) * distance;

        // Set the new position with the height offset
        transform.position = new Vector3(x, heightOffset, z) + target.position;

        // Calculate the downward angle direction
        Vector3 targetPositionWithOffset = target.position + new Vector3(0, heightOffset, 0);
        Vector3 direction = targetPositionWithOffset - transform.position;
        direction = Quaternion.Euler(downwardAngle, 0, 0) * direction;

        // Rotate the camera to look at the target with the downward angle
        transform.rotation = Quaternion.LookRotation(direction);
    }
}