using System.Collections;
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
        StartCoroutine(delayshowquiz());
        //QuizManager.Instance.ShowQuiz();
    }

    IEnumerator delayshowquiz()
    {
        yield return new WaitForSeconds(2f);
        QuizManager.Instance.ShowQuiz();
    }
    
}
