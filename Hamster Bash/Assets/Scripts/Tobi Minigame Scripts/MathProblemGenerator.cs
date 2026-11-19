
using UnityEngine;

public class MathProblemGenerator : MonoBehaviour
{
    public int difficulty = 1;

    public struct MathProblem
    {
        public string question;
        public int answer;
    }

    public MathProblem GenerateProblem()
    {
        MathProblem problem = new MathProblem();

        int a, b, c;

        switch (difficulty)
        {
            case 1:
                a = Random.Range(1, 21);
                b = Random.Range(1, 21);

                if (Random.value > 0.5f)
                {
                    problem.question = $"{a} + {b}";
                    problem.answer = a + b;
                }
                else
                {
                    if (a < b)
                    {
                        int temp = a;
                        a = b;
                        b = temp;
                    }

                    problem.question = $"{a} - {b}";
                    problem.answer = a - b;
                }
                break;

            case 2:
                a = Random.Range(2, 13);
                b = Random.Range(2, 13);

                if (Random.value > 0.5f)
                {
                    problem.question = $"{a} x {b}";
                    problem.answer = a * b;
                }
                else
                {
                    int answer = Random.Range(2, 13);
                    a = answer * b;

                    problem.question = $"{a} / {b}";
                    problem.answer = answer;
                }
                break;

            case 3:
                a = Random.Range(2, 11);
                b = Random.Range(2, 11);
                c = Random.Range(2, 6);

                problem.question = $"({a} + {b}) x {c}";
                problem.answer = (a + b) * c;
                break;

            case 4:
                a = Random.Range(2, 11);
                b = Random.Range(2, 11);

                if (Random.value > 0.5f)
                {
                    problem.question = $"{a}^2 + {b}";
                    problem.answer = (a * a) + b;
                }
                else
                {
                    int square = a * a;

                    problem.question = $"Sqrt({square}) + {b}";
                    problem.answer = a + b;
                }
                break;
        }

        return problem;
    }
}

