using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text continueInput;
    public Image scoreBackground;
    public GameObject raceOverPanel;
    public GameObject scorePanel;

    private float startTime; 
    private float raceTime;
    private float additionalTime = 0f; 
    private Coroutine flashCoroutine;

    private bool isFlashing; 
    private bool isRaceFinished = false;
    private bool isTimerStopped = false;
    private bool isRaceStarted = false;
    private Countdown countdownScript;
    

    private void Start()
    {
        countdownScript = GetComponent<Countdown>();
    }

    private void Update()
    {
        if (!countdownScript.countdownRunning)
        {
            scorePanel.gameObject.SetActive(true);
            Timer();
            //UIswitch();

            if (!isFlashing && isRaceFinished)
            {
                flashCoroutine = StartCoroutine(FlashText(scoreText));
                isFlashing = true;
                continueInput.enabled = true;
                ScoreManager.AddScore(raceTime);
                ScoreManager.SaveScores();
            }
            if (!isRaceFinished)
            {
                string formattedTime = FormatTime(raceTime);
                scoreText.text = formattedTime;
            }
            else if (isRaceFinished && Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
                scoreBackground.enabled = true;
                scoreText.enabled = true;
                continueInput.enabled = false;
                raceOverPanel.gameObject.SetActive(true);
                UpdateScoreText();
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level 1 (Snow)")
        {
            SceneManager.LoadScene("Level 2 (Sand)");
        }
        else if (currentSceneName == "Level 2 (Sand)")
        {
            SceneManager.LoadScene("Level 1 (Snow)");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Timer()
    {
        if (isRaceStarted && !isRaceFinished)
        {
            float elapsedTime = Time.time - startTime + additionalTime;

            if (!isRaceFinished)
            {
                raceTime = elapsedTime;
            }
        }
    }

    private string FormatTime(float time) //Format the time into minutes, seconds, and milliseconds
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000); //removing a zero from % 1000 removes 1 digit
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        return formattedTime;
    }

    private void UpdateScoreText()
    {
        List<float> topScores = ScoreManager.GetTopScores();

        string scoreString = "Top Scores:\n"; // Build the score text
        for (int i = 0; i < topScores.Count; i++)
        {
            string formattedScoreTime = FormatTime(topScores[i]);
            scoreString += $"{i + 1}. {formattedScoreTime}\n"; //Display scores in order
        }

        scoreText.text = scoreString;
    }

    private void OnEnable()
    {
        PlayerEvents.OnStartLine += RaceStart;
        PlayerEvents.OnFinishLine += RaceFinished;
        PlayerEvents.OnTimePenalty += PenaltyFlag;
        //Debug.Log("ScoreDisplay : " + "Subscribe to PlayerEvent"); 
    }

    private void OnDisable()
    {
        PlayerEvents.OnStartLine -= RaceStart;
        PlayerEvents.OnFinishLine -= RaceFinished;
        PlayerEvents.OnTimePenalty -= PenaltyFlag;
        //Debug.Log("ScoreDisplay : " + "Unsubscribe from PlayerEvent");
    }

    private void RaceStart()
    {
        startTime = Time.time;
        isRaceStarted = true;
    }

    private void RaceFinished()
    {
        //Debug.Log("ScoreDisplay : " + "RaceFinished");
        isRaceFinished = true;
    }

    private void PenaltyFlag()
    {
        additionalTime += 1f;
    }

    private IEnumerator FlashText(TMP_Text text)
    {
        while (true)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
}