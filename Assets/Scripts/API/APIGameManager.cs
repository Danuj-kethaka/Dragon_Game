using UnityEngine;

public class APIGameManager : MonoBehaviour
{
    public static APIGameManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void TriggerQuiz()
    {
        QuizManager.Instance.ShowQuiz();
    }
}
