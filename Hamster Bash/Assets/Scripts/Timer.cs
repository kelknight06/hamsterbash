using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    private float remainingTime;
    [SerializeField]
    private bool timerRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerRunning = false;
    }
    // Call to start timer with new remaining time
    public void StartTimer(float newTime)
    {
        remainingTime = newTime;
        timerRunning = true;
    }
    // Call to pause or resume timer. Timer will not resume if remaining time is 0.
    public void PauseResumeTimer()
    {
        if (timerRunning) { timerRunning = false; }
        else
        {
            if (remainingTime > 0) { timerRunning = true; }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!timerRunning) { return; } // Do nothing if timer is paused

        remainingTime -= Time.deltaTime; // Timer ticks down in seconds

        if (remainingTime < 0f) // Handles stopping timer if time runs out
        {
            remainingTime = 0f;
            timerRunning = false;
            UpdateTimerDisplay();

            // Can have code here to tell mini game about game over
            CarnivalShooter.Instance.StartPhase5();

            return;
        }
        UpdateTimerDisplay(); // Update timer display text
    }
    // Call to update TMPro text
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
