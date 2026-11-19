

using UnityEngine;
using TMPro;

public class MathGate : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text answerText;

    private bool isCorrect;
    private int pairID;

    public void Setup(
        string question,
        int answer,
        bool correct,
        int id)
    {
        questionText.text = question + " = ?";
        answerText.text = answer.ToString();

        isCorrect = correct;
        pairID = id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MathGameManager.Instance.ChooseGate(
                isCorrect,
                pairID
            );
        }
    }
}
