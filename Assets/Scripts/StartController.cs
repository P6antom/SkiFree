using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {   
            PlayerEvents.StartLine(); // If the trigger occurs with an object tagged as "Player", trigger the Finishline event
        }
    }
}