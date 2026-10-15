using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

// This is how the mini game will tell what state it is currently in
public enum GamePhase
{
    TutorialGoodTarget,
    TutorialBadTarget,
    WaitingToStart,
    Playing,
    Finished
}
// CarnivalShooter will act as the Mini Game's GameManager
public class CarnivalShooter : MonoBehaviour
{
    public static CarnivalShooter Instance;
    public TargetSpawner targetSpawner;
    public Timer gameTimer;

    [SerializeField]
    public GamePhase currentPhase;
    private int score; // Player's score
    public int difficulty = 1; // 1 - easy; 2 - normal; 3 - hard;
    private int targetScore; // Score needed to win
    private float timeLimit; // Time to reach target score in mini game

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject victoryText;
    [SerializeField]
    private GameObject targetScoreObject;
    private TextMeshProUGUI targetScoreText;

    [Header("Tutorial UI Text")]
    [SerializeField]
    private GameObject tutorialText1;
    [SerializeField]
    private GameObject tutorialText2;
    [SerializeField]
    private GameObject tutorialText3;
    

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() // Treating this almost like a StartPhase1 method for now
    {
        // Call on main GameManager here to figure out difficulty, then adjust targetScore and timeLimit accordingly
        score = 0;
        targetScoreText = targetScoreObject.GetComponent<TextMeshProUGUI>();
        targetScoreObject.SetActive(false);
        UpdateScore(0);
        currentPhase = GamePhase.TutorialGoodTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int points)
    {
        score += points;
        if (score <= 0) { score = 0; }

        scoreText.text = "Score " + score;
    }

    public void UpdateTargetScore(int points)
    {
        targetScore = points;

        if (targetScoreObject.activeInHierarchy)
        {
            targetScoreText.text = "Target Score:\n" + targetScore;
        }
    }

    // Remove tutorial good target and spawn tutorial bad target
    public void StartPhase2()
    {
        currentPhase = GamePhase.TutorialBadTarget;
        tutorialText1.SetActive(false);
        tutorialText2.SetActive(true);
        targetSpawner.BadTutorialTarget();
    }
    // Remove tutorial bad target and spawn start game target
    public void StartPhase3()
    {
        currentPhase = GamePhase.WaitingToStart;
        tutorialText2.SetActive(false);
        tutorialText3.SetActive(true);
        targetSpawner.StartGameTarget();

        targetScoreObject.SetActive(true); // Putting this here for now so the player is aware of the goal before the game starts
        UpdateTargetScore(4000);
    }
    // Remove start game target and start game. Begin spawning targets and start timer
    public void StartPhase4()
    {
        currentPhase = GamePhase.Playing;
        tutorialText3.SetActive(false);

        timeLimit = 120; // Eventually we can update this to reflect difficulty
        gameTimer.StartTimer(timeLimit);

        targetSpawner.BeginSpawning(difficulty);
    }
    // End game. Targets stop spawning and end result is displayed
    public void StartPhase5()
    {
        currentPhase = GamePhase.Finished;
        targetSpawner.StopSpawningTargets();

        if (score >= targetScore)
        {
            victoryText.SetActive(true); // Player Wins
        } else
        {
            gameOverText.SetActive(true); // Player Loses
        }

    }
}
