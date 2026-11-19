using UnityEngine;
using TMPro;

public class GameTimer1 : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    public float extraTimePerRound = 15f;
    public bool timerIsRunning = true;
    public bool gameIsWon = false;
    public MatchingManager matchingManager;

    [Header("UI Reference")]
    public TMP_Text timerText;
    
    [Header("Game Over")]
    public TMP_Text gameFailedText;
    [Header("Game Won")]
    public TMP_Text gameWonText;
    public MonoBehaviour playerMovement;
    public MonoBehaviour playerLook;


    void Update()
    {
        if (matchingManager.FinalRoundCompleted && !gameIsWon)
        {
            gameIsWon = true;
            timerIsRunning = false;
            gameWonText.gameObject.SetActive(true);

            playerMovement.enabled = false;
            PlayerLook lookScript = playerMovement.GetComponent<PlayerLook>();
            lookScript.enabled = false;
        }

        if (timerIsRunning) 
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);
                OnTimerEnd();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    

    public void OnTimerEnd()
    {
        Debug.Log("Time has run out!");

        gameFailedText.gameObject.SetActive(true);

        playerMovement.enabled = false;
        PlayerLook lookScript = playerMovement.GetComponent<PlayerLook>();
        lookScript.enabled = false;
    }

    public void AddRoundTime()
    {
        timeRemaining += extraTimePerRound;
        DisplayTime(timeRemaining);
    }
}
