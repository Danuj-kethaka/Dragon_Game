using System.Collections;
using UnityEngine;

// This class manages triggering the quiz system when player dies
public class APIGameManager : MonoBehaviour
{
    public static APIGameManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            // Singleton pattern to ensure only one instance exists.
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
        }
    }

    public void TriggerQuiz()
    {
        // Start a delay before showing quiz (event-driven behaviour)
        StartCoroutine(delayshowquiz());
    }

    IEnumerator delayshowquiz()
    {
        // Wait for 2 seconds before showing quiz UI
        yield return new WaitForSeconds(2f);
        QuizManager.Instance.ShowQuiz();
    }
    
}
