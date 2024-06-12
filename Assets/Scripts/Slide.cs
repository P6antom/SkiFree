using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;

public class Slide : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private Countdown countdownScript;
    public Slider boostSlider;
    //private AudioSource playerHit;
    private AudioSource audioSource;
    public AudioClip[] audioClips;
    private Rigidbody rb;

    //for player control
    private int playerDirection = 0;
    public float snapAngle = 45f; //angle between each snap point

    public float normalSpeed = 1f;
    private float knockBackVelocity = 30f;
    public bool FreezeMovement = false;

    //handles for boost
    public float boostMultiplier = 1.25f;
    public float boostAmount = 0.5f;
    public float boostTimer = 0f;
    public float boostCooldown = 2f;
    private bool isBoosting = false;
    private bool BoostOnCooldown = false;

    //for camera shake
    public float shakeDuration = 0.5f;
    public float shakeAmplitude = 10.2f;
    public float shakeFrequency = 20.0f;
    
    void Start()
    {
        GameObject uiCanvas = GameObject.Find("UICanvas");
        countdownScript = uiCanvas.GetComponent<Countdown>();
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = true;
        FreezeMovement = true;
    }

    void Update()
    {
        if (!countdownScript.countdownRunning)
        {
            FreezeMovement = false;
        }
        if(!FreezeMovement)
        {
            Sliding();
            BoostUI();
        }
    }

    private void Sliding()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        SpeedLimiter();

        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.rotation.eulerAngles.y > 90f) //snap angles for movement
        {
            transform.Rotate(0, -snapAngle, 0);
            //Debug.Log("playerDirection = " + playerDirection);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.rotation.eulerAngles.y < 270f)
        {
            transform.Rotate(0, snapAngle, 0);
            //Debug.Log("playerDirection = " + playerDirection);
        }

        SlowMovement();
        BoostLogic();

        if (rb.velocity.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[1]);
        }

        //Debug.Log("Player Speed: " + rb.velocity.magnitude);
    }

    private void SpeedLimiter()
    {
        float targetVelocityMagnitude = normalSpeed;
        float smoothingFactor = 10f;
        
        //limits player speed
        if (playerDirection == 1 || playerDirection == -1)
        {
            if (isBoosting)
                targetVelocityMagnitude = 40f;  //made speed limit larger for extra maneuverability
            else
                targetVelocityMagnitude = 25f;
        }   
        else if (playerDirection == 2 || playerDirection == -2)
        {
            if (isBoosting)
                targetVelocityMagnitude = 35f;
            else
                targetVelocityMagnitude = 20f;
        }
        else
        {
            if (isBoosting)
                targetVelocityMagnitude = 45f;
            else
                targetVelocityMagnitude = 30f;
        }

        if (rb.velocity.magnitude > targetVelocityMagnitude)
        {
            // Smoothly adjust velocity towards the target velocity
            Vector3 targetVelocity = transform.forward * targetVelocityMagnitude;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * smoothingFactor);
        }
        else
        {
            float acceleration = (targetVelocityMagnitude - rb.velocity.magnitude) * Time.deltaTime * 7f;

            if (isBoosting)
            {
                rb.velocity += transform.forward * acceleration * boostMultiplier;
            }
            else
            {
                rb.velocity += transform.forward * acceleration;
            }
        }
    }

    private void BoostLogic()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isBoosting && !BoostOnCooldown)
        {
            StartCoroutine(ActivateBoost());
        }

        if (isBoosting)
        {
            //reduce boost timer
            boostTimer -= Time.deltaTime;
            //Debug.Log("Boost Left : " + boostTimer);

            if (boostTimer <= 0)
            {
                isBoosting = false;
                boostTimer = 0;
                //Debug.Log("no boost left");
            }
        }
        else if (BoostOnCooldown)
        {
            boostTimer += Time.deltaTime;

            if (boostTimer >= boostCooldown)
            {
                BoostOnCooldown = false;
                boostTimer = 0;
            }
        }
    }

    void SlowMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerDirection >= -1)
            {
                playerDirection -= 1; //Subtract one on left arrow press
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerDirection <= 1)
            {
                playerDirection += 1; //Add one on right arrow press
            }
        }
    }

    //for playerevents
    private void OnEnable()
    {
        PlayerEvents.OnPlayerHit += Knockback; //Connects to the playerEvent.cs and if activated invokes KnockBack
        PlayerEvents.OnFinishLine += RaceFinished; //Connects to the playerEvent.cs and if activated invokes RaceFinished
        Debug.Log("Slide : " + "Subscribe to PlayerEvent"); 
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerHit -= Knockback;
        PlayerEvents.OnFinishLine -= RaceFinished;
        Debug.Log("Slide : " + "Unsubscribe from PlayerEvent");
    }

    private void Knockback()
    {
        rb.velocity = -transform.forward * knockBackVelocity;
        StartCoroutine(KnockOut());
        audioSource.PlayOneShot(audioClips[0]);
        //StartCoroutine(ShakeCamera());
        Debug.Log("Slide : " + "Knockback");
    }

    private void RaceFinished()
    {
        FreezeMovement = true;
        boostSlider.gameObject.SetActive(false);
        //StartCoroutine(ShakeCamera());
        Debug.Log("Slide : " + "RaceFinished");
    }

    private IEnumerator KnockOut()
    {
        FreezeMovement = true;
        yield return new WaitForSeconds(1f);
        FreezeMovement = false;
    }

    IEnumerator ActivateBoost()
    {
        isBoosting = true;
        BoostOnCooldown = true;
        boostTimer = boostAmount;

        yield return new WaitForSeconds(boostAmount); //how long the boost lasts

        isBoosting = false;
        yield return new WaitForSeconds(boostCooldown); //how long it takes to recharge
        BoostOnCooldown = false;
    }

    private void BoostUI()
    {
        if (isBoosting)
        {
            boostSlider.value = 0 + (boostTimer / boostAmount);
        }
        else if (BoostOnCooldown)
        {
            boostSlider.value = boostTimer / boostCooldown;
        }
        else
        boostSlider.value = 1;
    }
}