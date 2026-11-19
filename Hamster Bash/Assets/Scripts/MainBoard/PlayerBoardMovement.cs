using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBoardMovement : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [Header("Board References")]
    [SerializeField]
    private BoardManager boardManager;
    [SerializeField]
    private BoardSpace currentSpace;

    [Header("Movement Settings")]
    [SerializeField]
    [Tooltip("How quickly player object moves between spaces")]
    private float moveSpeed = 5f;
    [SerializeField]
    [Tooltip("Offset players height to meet board floor")]
    private float heightOffset = 1f;

    private int remainingSpaces;
    private BoardSpace selectedPath;

    private bool isMoving = false;

    public BoardSpace CurrentSpace => currentSpace;
    public bool IsMoving => isMoving;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Confirm player has starting position.
        if (currentSpace == null && boardManager != null)
        {
            currentSpace = boardManager.StartingSpace;
        }
        if (currentSpace == null)
        {
            Debug.LogError("Player has no starting space!", this);
            return;
        }

        // Place player at their starting space
        transform.position = currentSpace.transform.position + Vector3.up * heightOffset;

    }

    // Begin moving player set number of spaces
    public void MovePlayer(int spacesToMove)
    {
        // Check if player is already moving
        if (isMoving)
        {
            Debug.LogWarning("Player is already moving!");
            return;
        }
        // Check if number of spaces to move given is greater than 0
        if (spacesToMove <= 0)
        {
            Debug.LogWarning("Movement must be greater than zero!");
            return;
        }
        // Check if player movement is missing necessary references
        if (currentSpace == null || boardManager == null)
        {
            Debug.LogError("Player movement is missing references!");
            return;
        }

        // Using a coroutine to handle smooth player movement along board.
        StartCoroutine(MoveAlongBoard(spacesToMove));
    }

    // Player picks a path to go down.
    public void ChoosePath(BoardSpace space)
    {
        selectedPath = space;
    }

    private IEnumerator MoveAlongBoard(int spacesToMove)
    {
        isMoving = true;
        remainingSpaces = spacesToMove;

        while (remainingSpaces > 0)
        {
            // Get available space destinations
            var connections = boardManager.GetConnectedSpaces(currentSpace);
            // Check if no connections available
            if (connections.Count == 0)
            {
                Debug.Log("No more connected spaces!");
                break;
            }

            BoardSpace nextSpace;

            // If there are any branching paths, handle it here:
            if (connections.Count > 1)
            {
                Debug.Log("Reached Branching Path!");

                gameManager.RequestPathChoice(connections);

                selectedPath = null;

                while (selectedPath == null)
                {
                    yield return null;
                }

                nextSpace = selectedPath;
            }
            else
            {
                nextSpace = connections[0];

            }

            // Move towards next space
            Vector3 destination = nextSpace.transform.position + Vector3.up * heightOffset;

            while (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                yield return null;
            }

            // Snap to destination
            transform.position = destination;

            // Update player current space
            currentSpace = nextSpace;
            remainingSpaces--;

            Debug.Log("Player reached Space " + currentSpace.spaceID);
            Debug.Log("Remaining movement: " + remainingSpaces);
        }

        isMoving = false;

        Debug.Log("Movement finished. Current Space: " + currentSpace.spaceID);

        gameManager.OnPlayerLanded(currentSpace);
    }
}
