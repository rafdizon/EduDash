using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Collections;
using Firebase;
using TMPro;

public class LoginScript : MonoBehaviour
{
    [Space]
    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;

    public FirebaseUser user;

    private void Awake()
    {
        user = AuthManager.Instance.GetCurrentUser();
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(email.text, password.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = AuthManager.Instance.auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() =>  loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login failed: ";

            switch (authError) 
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Invalid email";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong password";
                    break;
                case AuthError.UserNotFound:
                    failedMessage += "User not found, signup first";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Enter email";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Enter password";
                    break;
                default:
                    failedMessage += "Unknown error, contact support";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result.User;
        }
    }
}
