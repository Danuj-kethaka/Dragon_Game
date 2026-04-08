using System;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;

public class SettingsPanel : MonoBehaviour
{
    public GameObject panelToOpen;
    private FirebaseAuth auth;
    public TMP_Text userEmailText;
    public TMP_Text userPasswordText;
    public TMP_Text userNameText;
    public TMP_Text maxScoreText;
    private FirebaseFirestore db;
    

      void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public void OpenPanel()
    {
        panelToOpen.SetActive(true);
        PlayerController.Instance.canMove = false;
        ShowUserInfo();
    }

    public void CLosePanel()
    {
        panelToOpen.SetActive(false);
        PlayerController.Instance.canMove = true;
    }

    // Show user information in the account panel
    public async Task ShowUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            userEmailText.text = "email: " + user.Email;
            userPasswordText.text = "password: ***************";
            
            DocumentReference docRef = db.Collection("users").Document(user.UserId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if(snapshot.Exists)
            {
                string username = "unknown";
                int score = 0;

                if(snapshot.ContainsField("username"))
                {
                    username = snapshot.GetValue<string>("username");
                }

                if(snapshot.ContainsField("score"))
                {
                    score = snapshot.GetValue<int>("score");
                }

                userNameText.text = "username : " + username;
                maxScoreText.text = "Max Score : " + score;
            }
        }
    }

}
