
using UnityEngine;
using TMPro;

public class MathGate : MonoBehaviour
{
    [Header("Gate UI")]
    public TMP_Text questionText;
    public TMP_Text answerText;

    private bool isCorrect;

    public void Setup(string question, int answer, bool correct)
    {
        questionText.text = question + " = ?";
        answerText.text = answer.ToString();

        isCorrect = correct;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MathGameManager.Instance.ChooseGate(isCorrect);
        }
    }
}
