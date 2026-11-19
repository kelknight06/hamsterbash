
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startingTime = 60f;

    [Header("UI")]
    public TMP_Text timerText;

    private float timeRemaining;
    private bool timerRunning = false;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateTimerUI();

            timerRunning = false;

            // End the minigame.
            MathGameManager.Instance.FinishMinigame();

            Debug.Log("Time is up!");
            return;
        }

        UpdateTimerUI();
    }

    public void StartTimer()
    {
        timeRemaining = startingTime;
        timerRunning = true;

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(timeRemaining);

        timerText.text = "Time: " + seconds;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}
