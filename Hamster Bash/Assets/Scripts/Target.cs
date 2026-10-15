using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    [SerializeField]
    private bool targetActive = false;
    [SerializeField]
    private int pointValue;
    [SerializeField]
    private float activeTime;
    private Coroutine targetTimer;

    // Create enum of target types that stores the four types of targets that can appear
    public enum TargetTypes
    {
        Inactive, // Does nothing
        Good,     // Gives points when shot
        Bad,      // Removes points when shot
        StartGame // Special Case; Appears once at beginning of game to trigger mini game start
    }

    [Header("Target Types")]
    [SerializeField]
    private TargetTypes selectedTypes;
    public string SelectedTypeString => selectedTypes.ToString();

    [Header("Target Renderer")]
    [SerializeField]
    private Renderer targetRenderer;

    // Materials used to visually represent each target type
    [Header("Target Materials")]
    [SerializeField]
    [Tooltip("Material used when the target is inactive.")]
    private Material inactiveMaterial;

    [SerializeField]
    [Tooltip("Material used for a good target.")]
    private Material goodMaterial;

    [SerializeField]
    [Tooltip("Material used for a bad target.")]
    private Material badMaterial;

    [SerializeField]
    [Tooltip("Material used for the start-game target.")]
    private Material startGameMaterial;

    // Mutator for changing a target's point value and active time
    public void AdjustValues(int newValue,  float newTime)
    {
        pointValue = newValue;
        activeTime = newTime;
    }

    // Method is used to change target types and activate them when needed
    public void ActivateTarget(TargetTypes targetType)
    {
        // If the targets active time is still running, end the timer coroutine
        if (targetTimer != null)
        {
            StopCoroutine(targetTimer);
            targetTimer = null;
        }

        selectedTypes = targetType;

        // Update target based on it's type. If it becomes active, timer coroutine starts.
        switch (selectedTypes)
        {
            case TargetTypes.Inactive:
                targetActive = false;
                targetRenderer.material = inactiveMaterial;
                break;
            case TargetTypes.Good:
                targetActive = true;
                targetRenderer.material = goodMaterial;
                if (CarnivalShooter.Instance.currentPhase == GamePhase.Playing)
                {
                    targetTimer = StartCoroutine(TargetTimer());
                }
                break;
            case TargetTypes.Bad:
                targetActive = true;
                targetRenderer.material = badMaterial;
                if (CarnivalShooter.Instance.currentPhase == GamePhase.Playing)
                {
                    targetTimer = StartCoroutine(TargetTimer());
                }
                break;
            case TargetTypes.StartGame:
                targetActive = true;
                targetRenderer.material = startGameMaterial;
                break;

        }
    }

    // Coroutine that runs the targets timer. When timer runs out, target becomes inactive
    private IEnumerator TargetTimer()
    {
        yield return new WaitForSeconds(activeTime);

        Destroy(gameObject);
    }

    // Method tells target it's been hit and reacts according to it's type
    public void TargetHit()
    {
        if (!targetActive) { return; }

        // Stop target timer coroutine if target is hit
        if (targetTimer != null)
        {
            StopCoroutine(targetTimer);
            targetTimer = null;
        }

        targetActive = false;

        switch (selectedTypes)
        {
            case TargetTypes.Good:
                if (CarnivalShooter.Instance.currentPhase == GamePhase.Playing) // Add points to score
                {
                    CarnivalShooter.Instance.UpdateScore(pointValue);
                    break;
                } else if (CarnivalShooter.Instance.currentPhase == GamePhase.TutorialGoodTarget) // Progress tutorial
                {
                    CarnivalShooter.Instance.UpdateScore(pointValue);
                    CarnivalShooter.Instance.StartPhase2();
                    break;
                }
                break;
            case TargetTypes.Bad:
                if (CarnivalShooter.Instance.currentPhase == GamePhase.Playing) // Remove points to score
                {
                    CarnivalShooter.Instance.UpdateScore(-pointValue);
                    break;
                }
                else if (CarnivalShooter.Instance.currentPhase == GamePhase.TutorialBadTarget) // Progress tutorial
                {
                    CarnivalShooter.Instance.UpdateScore(-pointValue);
                    CarnivalShooter.Instance.StartPhase3();
                    break;
                }
                break;
            case TargetTypes.Inactive:
                // Do nothing
                break;
            case TargetTypes.StartGame:
                // Start Mini Game
                CarnivalShooter.Instance.StartPhase4();
                break;

        }

        // Remove target after confirming it's effect
        Destroy(gameObject);
    }
}
