using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceBlock : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Place Dice Roll Button Here")]
    private Button rollDiceButton;

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
    }

    private void OnDisable()
    {
        if (rollDiceButton != null)
        {
            rollDiceButton.onClick.RemoveListener(OnRollDice);
        }
    }

    private void OnRollDice()
    {
        if (body == null)
        {
            return;
        }

        body.isKinematic = false;

        forceX = Random.Range(0, maxRandomForceValue);
        forceY = Random.Range(0, maxRandomForceValue);
        forceZ = Random.Range(0, maxRandomForceValue);

        body.AddForce(Vector3.up * startRollingForce);
        body.AddTorque(forceX, forceY, forceZ);
    }

    private void Initialize()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        transform.rotation = Random.rotation;
        diceFaceNum = 0;
    }
}
