
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;


// BoardManager is responsible for reading the entire gameboard.
// It should be placed on a Board object that contains a BoardSpaces object which contains the board's spaces.
public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField]
    [Tooltip("Link First Space Here\nFirst space should be at top of heirarchy and within a boardSpaces parent object.")]
    private BoardSpace startingSpace;

    [UnitHeaderInspectable("Board Info")]
    [SerializeField]
    private List<BoardSpace> boardSpaces = new List<BoardSpace>();

    public BoardSpace StartingSpace => startingSpace;
    public IReadOnlyList<BoardSpace> BoardSpaces => boardSpaces;


    // On awake, BoardManager finds every board space within the board object
    private void Awake()
    {
        boardSpaces.Clear();

        boardSpaces.AddRange(GetComponentsInChildren<BoardSpace>(true));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ValidateBoard();
    }


    // Retrieve a space from it's ID
    public BoardSpace GetSpaceID(int id)
    {
        foreach (BoardSpace space in boardSpaces)
        {
            if (space.spaceID == id)
            {
                return space;
            }
        }
        Debug.LogWarning("No BoardSpace found with ID: " + id);
        return null;
    }

    // Retrieve all possible spaces that can be moved from a given space
    public IReadOnlyList<BoardSpace> GetConnectedSpaces(BoardSpace space)
    {
        if (space == null)
        {
            return System.Array.Empty<BoardSpace>();
        }

        return space.connectedSpaces;
    }


    // This method is run to check if the board has been setup correctly. This should help confirming the configuration of a new board.
    private void ValidateBoard()
    {
        // Check if board is empty
        if (boardSpaces.Count == 0)
        {
            Debug.LogError("Board contains no spaces!", this);
            return;
        }
        // Check if board has a starting space
        if (startingSpace == null)
        {
            Debug.LogError("Board has no starting space!", this);
        }
        else if (!boardSpaces.Contains(startingSpace)) // Check if starting space is start of space set
        {
            Debug.LogError("Starting space has not been set!", this);
        }

        HashSet<int> usedIDs = new HashSet<int>();

        foreach (BoardSpace space in boardSpaces)
        {
            if (space == null)
            {
                continue;
            }

            // Check for duplicate IDs
            if (!usedIDs.Add(space.spaceID))
            {
                Debug.LogError("Duplicate BoardSpace ID: " + space.spaceID, space);
            }

            // Check space connections
            foreach (BoardSpace connection in space.connectedSpaces)
            {
                if (connection == null)
                {
                    Debug.LogError("A space has no connections!", space);
                }
                else if (!boardSpaces.Contains(connection))
                {
                    Debug.LogError("Space " + space.spaceID + " connects to space outside board!", space);
                }
            }
        }

        Debug.Log("Board Validation Complete. Found " +  boardSpaces.Count + " spaces.", this);
    }
}
