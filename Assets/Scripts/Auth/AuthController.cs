using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class AuthController : MonoBehaviour
{
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginStatusText;
    public TMP_Text logoutStatusText;
    
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TMP_Text registerstatusText;
    public TMP_Text userEmailText;

    public GameObject loginPanel;
    public GameObject panelToOpen;
    public GameObject AdminPanel;
    private FirebaseAuth auth;
    private FirebaseFirestore db;
    public Slider AccountSlider;

    async void Start()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        AccountSlider.interactable = false;

        db = FirebaseFirestore.DefaultInstance;

        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("Firebase ready");
        }
        else
        {
            Debug.LogError("Could not resolve dependencies");
        }
    }


    public void OnLoginClicked()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;
        LoginUser(email,password);
    }

    async void LoginUser(string email,string password)
    {
        if(email == "" || password == "")
        {
            loginStatusText.text = "Login fields cannot be empty";
            return;
        }

        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email,password);
            FirebaseUser user = result.User;
            await user.ReloadAsync();
            if(user.IsEmailVerified)
            {
               loginStatusText.text = "Login Successfully";
               StartCoroutine(ClearLoginMessage());
               loginPanel.SetActive(false);
               AccountSlider.interactable= true;
               PlayerController.Instance.canMove = true;
               Debug.Log("User logged in: "+result.User.Email); 
               await CheckUserRole(user.UserId);
            }
        }
        catch(System.Exception e)
        {
            loginStatusText.text = "Login Failed";
            Debug.LogError(e.Message);
        }
    }

    async Task CheckUserRole(string UserId)
    {
        DocumentReference docRef = db.Collection("users").Document(UserId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if(snapshot.Exists)
        {
            string role = snapshot.GetValue<string>("role");
            if(role == "admin")
            {
                AdminPanel.SetActive(true);
                PlayerController.Instance.canMove = false;
            }
        }
    }

    IEnumerator ClearLoginMessage()
    {
        yield return new WaitForSeconds(2f);
        loginStatusText.text = "";
    }

    public void OnRegisterClicked()
    {
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

        RegisterUser(email,password);
    }

    async void RegisterUser(string email,string password)
    {
        if(email == "" || password == "")
        {
            registerstatusText.text = "Register fields cannot be empty";
            return;
        }

        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email,password);
            FirebaseUser user = result.User;
            await user.SendEmailVerificationAsync();
            DocumentReference docRef = db.Collection("users").Document(user.UserId);
            Dictionary<string,object> data = new Dictionary<string, object>()
            {
                {"email",email},
                {"role","player"},
                {"score",0}
            };
            await docRef.SetAsync(data);
            registerstatusText.text = "Account created Successfully.We have sent you a confirmation link to your email.verify your account to continue.";
            Debug.Log("User registered: "+result.User.Email);
        }
        catch(System.Exception e)
        {
            registerstatusText.text = "Registration Failed";
            Debug.LogError(e.Message);
        }
    }

    public void SignOutUser()
    {
        if(auth!=null)
        {
            auth.SignOut();
            logoutStatusText.text = "User log out Successfully";
            loginPanel.SetActive(true);
            panelToOpen.SetActive(false);
            AccountSlider.interactable=false;
            loginEmailInput.text = "";
            loginPasswordInput.text = "";
            PlayerController.Instance.canMove = false;   
        }
    }

}
