using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Check whether the object entering the trigger is a dice
        DiceBlock dice = other.GetComponentInParent<DiceBlock>();

        if (dice == null)
        {
            return;
        }

        // Get the Rigidbody attached to the dice
        Rigidbody diceBody = dice.GetComponent<Rigidbody>();

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
                dice.diceFaceNum = faceNumber;
            }
        }
    }
}
