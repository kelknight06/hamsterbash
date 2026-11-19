using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject diceBlock;
    [SerializeField]
    private Dice dice;

    private void OnTriggerStay(Collider other)
    {
        if (diceBlock == null)
        {
            return;
        }

        // Get the Rigidbody attached to the dice
        Rigidbody diceBody = diceBlock.GetComponent<Rigidbody>();
        if (diceBody == null)
        {
            return;
        }

        // Only detect the face when the dice has stopped moving
        if (diceBody.linearVelocity.sqrMagnitude < 0.001f && diceBody.angularVelocity.sqrMagnitude < 0.001f)
        {
            // Convert the collider's name into a face number
            if (int.TryParse(other.name, out int faceNumber))
            {
                dice.rollValue(faceNumber);
            }
        }
    }
}