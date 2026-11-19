using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class MathGameManager : MonoBehaviour
{
    public static MathGameManager Instance;

    private MinigameSceneController minigameSceneController;

    // Results accessible by the main game manager.
    public static bool LastMinigameWon { get; private set; }
    public static int LastMinigameScore { get; private set; }
    public static bool HasResult { get; private set; }

    [Header("Game References")]
    public MathProblemGenerator generator;
    public MathGate leftGate;
    public MathGate rightGate;

    [Header("Score Settings")]
    public int score = 0;
    public int correctPoints = 10;
    public int wrongPenalty = 5;
    public int minimumWinningScore = 50;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text resultText;

    [Header("Difficulty")]
    public int difficulty = 1;

    private bool gateSelected = false;
    private bool gameFinished = false;

    private int nextPairID = 0;

    private HashSet<int> answeredPairs =
        new HashSet<int>();

    public bool GameFinished => gameFinished;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartNewPlaythrough();
    }

    public void StartNewPlaythrough()
    {
        // Increase difficulty each time the minigame starts.
        int previousDifficulty =
            PlayerPrefs.GetInt("MathDifficulty", 0);

        difficulty = Mathf.Clamp(
            previousDifficulty + 1, 1, 4
        );

        PlayerPrefs.SetInt("MathDifficulty", difficulty);
        PlayerPrefs.Save();

        generator.difficulty = difficulty;

        score = 0;
        gameFinished = false;
        gateSelected = false;

        HasResult = false;

        UpdateScoreUI();
       

        Debug.Log("Difficulty Level: " + difficulty);
    }


    public void GenerateNextGatePair()
    {
        if (gameFinished)
            return;

        int pairID = nextPairID++;

        // Generate a math problem.
        MathProblemGenerator.MathProblem problem =
            generator.GenerateProblem();

        // Randomly choose the correct path.
        bool leftIsCorrect = Random.value > 0.5f;

        int wrongAnswer = GenerateWrongAnswer(problem.answer);

        leftGate.Setup(
            problem.question,
            leftIsCorrect ? problem.answer : wrongAnswer,
            leftIsCorrect,
            pairID
        );

        rightGate.Setup(
            problem.question,
            leftIsCorrect ? wrongAnswer : problem.answer,
            !leftIsCorrect,
            pairID
        );
    }


    private int GenerateWrongAnswer(int correctAnswer)
    {
        int offset = Random.Range(1, 6);

        if (correctAnswer > offset && Random.value > 0.5f)
        {
            return correctAnswer - offset;
        }

        return correctAnswer + offset;
    }


    public void ChooseGate(bool correct, int pairID)
    {
        if (gameFinished || !answeredPairs.Add(pairID))
            return;

        if (correct)
        {
            score += correctPoints;
            Debug.Log("Correct! +10 points");
        }
        else
        {
            score -= wrongPenalty;
            Debug.Log("Incorrect! -5 points");
        }

        UpdateScoreUI();
    }


    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Call this when the player reaches the finish line.
    public void FinishMinigame()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        // Determine whether the player won.
        bool won = score >= minimumWinningScore;

        // Save results for the main game manager.
        LastMinigameScore = score;
        LastMinigameWon = won;
        HasResult = true;

        if (resultText != null)
        {
            resultText.text = won ? "YOU WIN!" : "YOU LOSE!";
        }

        Debug.Log("Final Score: " + score);
        Debug.Log("Minigame Result: " + (won ? "WIN" : "LOSS"));

        Debug.Log("Game End");

        StartCoroutine(EndMinigame());
    }

    // Reset difficulty for testing.
    public void ResetDifficulty()
    {
        PlayerPrefs.DeleteKey("MathDifficulty");
        difficulty = 1;
    }



    public void SetupGatePair(MathGate leftGate, MathGate rightGate)
    {
        if (gameFinished)
            return;

        int pairID = nextPairID++;

        MathProblemGenerator.MathProblem problem =
            generator.GenerateProblem();

        bool leftIsCorrect = Random.value > 0.5f;

        int wrongAnswer = GenerateWrongAnswer(problem.answer);

        leftGate.Setup(
            problem.question,
            leftIsCorrect ? problem.answer : wrongAnswer,
            leftIsCorrect,
            pairID
        );

        rightGate.Setup(
            problem.question,
            leftIsCorrect ? wrongAnswer : problem.answer,
            !leftIsCorrect,
            pairID
        );
    }

    private IEnumerator EndMinigame()
    {
        Debug.Log("Telling minigameSceneController to end game");
        yield return new WaitForSeconds(4);

        minigameSceneController.FinishMinigame(LastMinigameWon);
    }

}