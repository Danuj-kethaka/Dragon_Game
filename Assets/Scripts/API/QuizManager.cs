using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;
    [SerializeField] private GameObject quizpanel;
    [SerializeField] private TMPro.TMP_Text questionText;
    [SerializeField] private TMPro.TMP_InputField answerInput;
    [SerializeField] private TMPro.TMP_Text resultText;
    [SerializeField] private RawImage puzzleImage;

    private string correctAnswer;
    
    void Awake()
    {
        Instance = this;
    }

    public void ShowQuiz()
    {
        quizpanel.SetActive(true);
        StartCoroutine(GetQuizQuestion());
        puzzleImage.texture = null;
    }

    IEnumerator GetQuizQuestion()
    {
        resultText.text = "";
        string url = "https://marcconrad.com/uob/banana/api.php";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            BananaResponse response = JsonUtility.FromJson<BananaResponse>(json);
            questionText.text = "Solve the puzzle to continue the game  !!!!!!";
            correctAnswer = response.solution.ToString();
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

    public void SubmitAnswer()
    {
        if(answerInput.text == correctAnswer)
        {
            resultText.text = "correct...health restored";
            PlayerController.Instance.RestoreHealth();
            quizpanel.SetActive(false);
        }

        else
        {
            StartCoroutine(ShowWrongAnswer());
        }
    }
    IEnumerator ShowWrongAnswer()
    {
        resultText.text = "Wrong answer!";
        yield return new WaitForSeconds(2f);
        resultText.text = "";
        answerInput.text = "";
    }
}

[System.Serializable]
public class BananaResponse
{
    public string question;
    public int solution;
}