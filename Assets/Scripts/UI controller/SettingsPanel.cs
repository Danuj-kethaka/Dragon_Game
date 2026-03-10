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
        PlayerController.Instance.canMove = false;
        ShowUserInfo();
    }

    public void CLosePanel()
    {
        panelToOpen.SetActive(false);
        PlayerController.Instance.canMove = true;
    }

    public void ShowUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            userEmailText.text = "email: " + user.Email;
            userPasswordText.text = "password: ***************";

        }
    }

}
