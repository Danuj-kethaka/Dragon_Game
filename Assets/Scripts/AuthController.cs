using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class AuthController : MonoBehaviour
{
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginStatusText;
    
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TMP_Text registerstatusText;

    public GameObject loginPanel;

    private FirebaseAuth auth;

    async void Start()
{
    var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

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
            loginStatusText.text = "Login Successfully";
            loginPanel.SetActive(false);
            Debug.Log("User logged in: "+result.User.Email);
        }
        catch(System.Exception e)
        {
            loginStatusText.text = "Login Failed";
            Debug.LogError(e.Message);
        }
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
            registerstatusText.text = "User Registered Successfully";
            Debug.Log("User registered: "+result.User.Email);
        }
        catch(System.Exception e)
        {
            registerstatusText.text = "Registration Failed";
            Debug.LogError(e.Message);
        }
    }

}
