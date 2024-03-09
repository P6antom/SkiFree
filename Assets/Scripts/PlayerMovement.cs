using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float snapAngle = 45f;
    public float downForce = 1f;
    public float velocity = 5f;
    public float maxVelocity = 10;
    public float rotationSpeed = 10f;

    private bool isFacingLeft = false;
    
    private float playerDirection = 180f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get input from A and D keys (or left and right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the new rotation angle
        float targetRotation = isFacingLeft ? 90f : -90f;

        // Rotate the player towards the target angle
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        // Clamp the rotation within the 180-degree arc
        float currentRotation = transform.eulerAngles.y;
        float clampedRotation = Mathf.Clamp(currentRotation, -90f, 90f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, clampedRotation, transform.eulerAngles.z);

        // Update the facing direction based on the rotation
        isFacingLeft = clampedRotation > 0f;

        //rb.velocity = transform.forward * velocity * Time.deltaTime;

        // if ()
        // {

        // }

        // Adjust the velocity based on the player's direction
        velocity = AdjustVelocity(playerDirection, velocity);

        // Apply the velocity to move the player
        Vector3 moveDirection = Quaternion.Euler(0, playerDirection, 0) * Vector3.forward;
        rb.velocity = moveDirection * velocity;

        // Limit the velocity to the maximum velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        
        rb.velocity += Vector3.down * downForce * Time.deltaTime; //downforce for the gravity
    }

    float AdjustVelocity(float direction, float currentVelocity)
    {
        if (direction == 180f)
        {
            return currentVelocity; // No change in velocity
        }
        else if (direction == 270f || direction == 90f)
        {
            return 0f; // Velocity is multiplied by zero
        }
        else if (direction == 225f || direction == 135f)
        {
            return currentVelocity / 2f; // Velocity is multiplied by half
        }
        else
        {
            return currentVelocity; // Default velocity
        }
    }

    // void Sliding()
    // {
    //     rb.velocity += Vector3.down * downForce * Time.deltaTime; //downforce for going smoothly down the slope
    //     rb.velocity += transform.forward * velocity * Time.deltaTime; //moving the player forward
    // }
}
