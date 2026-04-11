using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;

// Controls the Banana API quiz panel
public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;
    [SerializeField] private GameObject quizpanel;
    [SerializeField] private TMPro.TMP_Text questionText;
    [SerializeField] private TMPro.TMP_InputField answerInput;
    [SerializeField] private TMPro.TMP_Text resultText;
    [SerializeField] private RawImage puzzleImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private TMPro.TMP_Text attemptsText;
    [SerializeField] private GameObject loginPanel;

    private string correctAnswer;
    private int wrongAttempts = 0;
    private FirebaseAuth auth;
    private FirebaseFirestore db;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public void ShowQuiz()
    {
        quizpanel.SetActive(true);
        wrongAttempts = 0;
        attemptsText.text = "Attempts Left: 2";
        StartCoroutine(GetQuizQuestion());
        puzzleImage.texture = null;
    }

    IEnumerator GetQuizQuestion()
    {
        resultText.text = "";
        // Calling external Banana API to fetch quiz question (interoperability)
        string url = "https://marcconrad.com/uob/banana/api.php";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            // Convert JSON response into C# object
            BananaResponse response = JsonUtility.FromJson<BananaResponse>(json);
            questionText.text = "Solve the puzzle to continue the game  !!!!!!";
            correctAnswer = response.solution.ToString();
            // Load puzzle image from API URL
            StartCoroutine(LoadPuzzleImage(response.question));
        }

        else
        {
            resultText.text = "Network Error";
        }
    }

   IEnumerator LoadPuzzleImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            puzzleImage.texture = texture;
        }
        else
        {
            resultText.text = "Image Load Failed";
        }
    }

    // Event - called when user presses sumbit button in UI
    public void SubmitAnswer()
    {
        // Event triggered when user sumbits answer
        if(answerInput.text == correctAnswer)
        {
            resultText.text = "correct...health restored";
            wrongAttempts = 0;
            PlayerController.Instance.RestoreHealth();
            quizpanel.SetActive(false);
        }

        else
        {
            wrongAttempts++;
            int remainingAttempts = 2-wrongAttempts;
            attemptsText.text = "Attempts Left: "+remainingAttempts;
            if(wrongAttempts>=2)
            {
                GameOver();
            }
            else
            {
                StartCoroutine(ShowWrongAnswer());
            }
        }
    }

    public void GameOver()
    {
        quizpanel.SetActive(false);
        int score = GameManager.Instance.killCount;
        gameOverPanel.SetActive(true);
        scoreText.text = "Your Score : " + score;
        SaveScore(score);
    }

    // Save score in Firebase Firestore database 
    async void SaveScore(int newScore)
    {
        FirebaseUser user = auth.CurrentUser;
        if(user != null)
        {
            DocumentReference docRef = db.Collection("users").Document(user.UserId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            int oldScore = 0;

            if(snapshot.Exists && snapshot.ContainsField("score"))
            {
                oldScore = snapshot.GetValue<int>("score");
            } 
            
            // Check and update only if new score is higher
            if(newScore>oldScore)
            {
                 Dictionary<string,object> updates = new Dictionary<string, object>()
                {
                    {"score", newScore}
                };
                await docRef.UpdateAsync(updates);
                Debug.Log("New high Score saved in the database");
            }
            else
            {
                Debug.Log("Score not higher than old score");
            }
        }
    }

    IEnumerator ShowWrongAnswer()
    {
        resultText.text = "Wrong answer!";
        yield return new WaitForSeconds(2f);
        resultText.text = "";
        answerInput.text = "";
    }

    public void restart()
    {
        gameOverPanel.SetActive(false);
        wrongAttempts = 0;
        PlayerController.Instance.RestoreHealth();
        GameManager.Instance.killCount = 0;
        BackgroundController.Instance.UpdatekillSlider(0, GameManager.Instance.maxKills);
    }

    public void Home()
    {
        auth.SignOut();
        gameOverPanel.SetActive(false);
        loginPanel.SetActive(true);
        PlayerController.Instance.canMove = false;
        PlayerController.Instance.RestoreHealth();
        GameManager.Instance.killCount = 0;
        BackgroundController.Instance.UpdatekillSlider(0,GameManager.Instance.maxKills);
    }
}

[System.Serializable]
public class BananaResponse
{
    public string question;
    public int solution;
}