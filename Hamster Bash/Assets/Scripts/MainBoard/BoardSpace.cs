using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SpaceType
{
    Normal, // Nothing happens, roll again, maybe earn some points
    Minigame, // Play mini game, must complete mini game to move again
    Special // Special event oocurs?
}

// BoardSpace is a class used to define a single board space and all it's information.
public class BoardSpace : MonoBehaviour
{
    [Header("Information")]
    [Tooltip("The Space's Identification")]
    public int spaceID;
    [Tooltip("The Space's Type")]
    public SpaceType spaceType; // See enum at top.

    [UnitHeaderInspectable("Connections")]
    [Tooltip("Spaces the player can move to from this one")]
    public List<BoardSpace> connectedSpaces = new List<BoardSpace>();
}
