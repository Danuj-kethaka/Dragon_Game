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
        // Loads all users from Firestore and display them in admin panel
        LoadUsers();
    }

    async void LoadUsers()
    {
        // Fetch all users from database (cloud data retrieval)
        QuerySnapshot snapshot = await db.Collection("users").GetSnapshotAsync();
        foreach(DocumentSnapshot doc in snapshot.Documents)
        {
            string email = "";
            string username = "empty";
            int score = 0;

            if(doc.ContainsField("email"))
            {
                email = doc.GetValue<string>("email");
            }
            if(doc.ContainsField("username"))
            {
                username = doc.GetValue<string>("username");
            }
            if(doc.ContainsField("score"))
            {
                score = doc.GetValue<int>("score");
            }

            GameObject textObj = Instantiate(userTextPrefab,content);
            textObj.GetComponent<TMP_Text>().text = username + " | " + email + " | " + "Score " + score;
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
