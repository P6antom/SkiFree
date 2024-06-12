using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public AudioClip whistleSound;
    public TMP_Text countdownText;
    public float countdownDuration = 3f;
    public float scaleDuration = 0.5f;

    public bool countdownRunning = true;
    private AudioSource audioSource;

    private void Start()
    {
        countdownRunning = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Vector3 initialScale = countdownText.transform.localScale;
        Vector3 minScale = initialScale * 0.5f;

        for (int i = (int)countdownDuration; i > 0; i--) //countdown loop
        {
            countdownText.text = i.ToString();

            yield return ScaleText(countdownText, minScale, scaleDuration / 8f); //Scale down


            yield return ScaleText(countdownText, initialScale, scaleDuration / 1.5f); //Scale up
            audioSource.PlayOneShot(whistleSound);

            yield return new WaitForSeconds(0.5f);
        }

        countdownText.text = "Go!";
        audioSource.PlayOneShot(whistleSound);
        
        countdownRunning = false; //activates other scripts

        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
    }

    private IEnumerator ScaleText(TMP_Text text, Vector3 targetScale, float duration)
    {
        Vector3 startScale = text.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        text.transform.localScale = targetScale; //checks if target scale is reached
    }

    public bool IsCountdownRunning()
    {
        return countdownRunning;
    }
}
