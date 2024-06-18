using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private string playerTag = "Player";

    public bool isDestructible = true;
    public bool hasParticles = false;
    public ParticleSystem particleSystem; // Reference to the particle system component
    public float destructionDelay = 2f; // Delay before destroying the GameObject
    public GameObject objectToFade;
    public float fadeDuration = 2f;
    public GameObject objectToAnimate;
    private Collider objectCollider;

    private void Start()
    {
        objectCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            PlayerEvents.PlayerHit();
            
            

            if (isDestructible)
            {
                if (hasParticles && particleSystem != null)
                {
                    particleSystem.Play();
                    StartCoroutine(DestroyWithDelay());
                    objectToAnimate.GetComponent<Animator>().SetTrigger("Knocked over");
                    objectCollider.enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator DestroyWithDelay()
    {
        // Wait for the destruction delay
        yield return new WaitForSeconds(destructionDelay);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}