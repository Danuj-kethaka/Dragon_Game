using UnityEngine;
using Firebase.Firestore;
using TMPro;
using System.Collections.Generic;
using Firebase.Auth;

public class AdminManager : MonoBehaviour
{
    public GameObject userTextPrefab;
    public GameObject AdminPanel;
    public GameObject loginPanel;
    public Transform content;
    private FirebaseAuth auth;
    public TMP_Text logoutStatusText;
    FirebaseFirestore db;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        LoadUsers();
    }

    async void LoadUsers()
    {
        QuerySnapshot snapshot = await db.Collection("users").GetSnapshotAsync();
        foreach(DocumentSnapshot doc in snapshot.Documents)
        {
            string email = doc.GetValue<string>("email");
            int score = 0;
            if(doc.ContainsField("score"))
            {
                score = doc.GetValue<int>("score");
            }
            GameObject textObj = Instantiate(userTextPrefab,content);
            textObj.GetComponent<TMP_Text>().text = email+" | score  : "+  score;
        }
    }

    public void SignOut()
    {
        if(auth!=null)
        {
           auth.SignOut();
           logoutStatusText.text = "User log out Successfully";
           AdminPanel.SetActive(false);
           loginPanel.SetActive(true); 
        }
    }


}
