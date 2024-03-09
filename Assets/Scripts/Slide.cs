using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{

    public float normalSpeed = 10f;
    public float boostSpeed = 15f;
    public float snapAngle = 45f; //angle between each snap point
    public float downForce = 5f; //fake gravity
    public float facingLeftSpeedMultiplier = 0.5f; //slows the momentum

    private bool facingRight = true;
    private bool speedBoost = false;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");

        //for sliding down the slope
        rb.velocity += Vector3.down * downForce * Time.deltaTime;
        rb.velocity += transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity += transform.forward * boostSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity += transform.forward * nromalSpeed * Time.deltaTime;
        }

        //Snap rotation to the nearest snap angle only when a key is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.rotation.eulerAngles.y > 90f)
            transform.Rotate(0, -snapAngle, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.rotation.eulerAngles.y < 270f)
            transform.Rotate(0, snapAngle, 0);

        //Update facing direction
        if (horizontalInput > 0)
            facingRight = true;
        else if (horizontalInput < 0)
            facingRight = false;

        //Speed modifiers
        if (!facingRight)
            rb.velocity *= facingLeftSpeedMultiplier;
        else if (speedBoost)
            rb.velocity *= boostAmount;
        else
            rb.velocity *= 1f;
    }
}