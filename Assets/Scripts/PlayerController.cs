using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    //public float snapAngle = 45f; // Angle between each snap point
    public float downForce = 5f; // Adjust the constant downward force
    
    public float rotationSpeed = 50f; // Speed of rotation
    public float maxRotationAngle = 90f; // Maximum rotation angle (half of the 180-degree arc)

    private Rigidbody rb;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's gravity
        rb.freezeRotation = true; // Prevent rigidbody from rotating
    }

    void Update()
    {
        Sliding();

        // Get input for rotation
        float rotationInput = Input.GetAxis("Horizontal");

        // Calculate rotation angle based on input
        float targetRotationAngle = Mathf.Clamp(rotationInput * maxRotationAngle, -maxRotationAngle, maxRotationAngle);

        // Calculate rotation step based on rotation speed and time
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Rotate the player around the y-axis
        transform.Rotate(0, targetRotationAngle * rotationStep, 0);

        // // Get input for left and right movement
        // float horizontalInput = Input.GetAxis("Horizontal");

        // // Calculate the target snap angle based on input
        // float targetAngle = Mathf.Round(transform.eulerAngles.y / snapAngle) * snapAngle;

        // // Snap rotation to the nearest snap angle
        // transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        // // Update facing direction
        // facingRight = horizontalInput > 0;

        // // Slow down movement depending on facing direction
        // if (!facingRight)
        //     rb.velocity *= 0.5f;
    }

    private void Sliding()
    {

        //vector3 velocity 
        //velocity.y = rb.velocity.y
        //rb.addforece = velocity

        rb.velocity += Vector3.down * downForce * Time.deltaTime; //downforce for going smoothly down the slope
        rb.velocity += transform.forward * movementSpeed * Time.deltaTime;  //move the player forward constantly
    }

    private void RotatePlayer()
    {
        // raycast
        // if (onground)
        // {
        //     turn left

        // }
    }
}