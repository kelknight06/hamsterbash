using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Place Dice Block Object Here")]
    private GameObject diceBlock;

    [SerializeField]
    [Tooltip("Place Dice Roll Button Here")]
    private Button rollDiceButton;

    [SerializeField]
    [Tooltip("Place Start Movement Button Here")]
    private Button startMovementButton;

    private GameManager gameManager;

    private Rigidbody body;

    [SerializeField]
    private float maxRandomForceValue, startRollingForce;

    private float forceX, forceY, forceZ;

    public int diceFaceNum;

    public void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (rollDiceButton != null)
        {
            rollDiceButton.onClick.AddListener(OnRollDice);
        }
        if (startMovementButton != null)
        {
            startMovementButton.onClick.AddListener(OnStartMovement);
        }
    }

    private void OnDisable()
    {
        if (rollDiceButton != null)
        {
            rollDiceButton.onClick.RemoveListener(OnRollDice);
        }
        if (startMovementButton != null)
        {
            startMovementButton.onClick.RemoveListener(OnStartMovement);
        }
    }

    private void OnRollDice()
    {
        if (body == null)
        {
            return;
        }

        body.isKinematic = false;

        rollDiceButton.gameObject.SetActive(false);

        forceX = Random.Range(0, maxRandomForceValue);
        forceY = Random.Range(0, maxRandomForceValue);
        forceZ = Random.Range(0, maxRandomForceValue);

        body.AddForce(Vector3.up * startRollingForce);
        body.AddTorque(forceX, forceY, forceZ);
    }

    public void OnStartMovement()
    {
        gameManager.MovePlayer(diceFaceNum);
        Destroy(gameObject);
    }

    public void rollValue(int diceValue)
    {
        diceFaceNum = diceValue;

        // spawn dice roll result flyby text using diceValue here

        startMovementButton.gameObject.SetActive(true);
    }

    private void Initialize()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        body = diceBlock.GetComponent<Rigidbody>();
        body.isKinematic = true;
        diceBlock.transform.rotation = Random.rotation;
        diceFaceNum = 0;
    }
}