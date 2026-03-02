using System;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class SettingsPanel : MonoBehaviour
{
    public GameObject panelToOpen;
    private FirebaseAuth auth;
    public TMP_Text userEmailText;
    public TMP_Text userPasswordText;
    

      void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void OpenPanel()
    {
        panelToOpen.SetActive(true);
        ShowUserInfo();
    }

    public void CLosePanel()
    {
        panelToOpen.SetActive(false);
    }

    public void ShowUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            userEmailText.text = "Email : " + user.Email;
            userPasswordText.text = "password : ***************";

        }
    }

}
