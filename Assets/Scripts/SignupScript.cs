using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Firebase.Firestore;
using System.Collections.Generic;
using System;
public class SignupScript : MonoBehaviour
{
    [Space]
    [Header("Signup")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField repeatPasswordField;
    public TMP_InputField usernameField;
    public TextMeshProUGUI errorText;
    public Button signupBtn;
    public FirebaseUser user;
    FirebaseFirestore db;

    private void Awake()
    {
        user = AuthManager.Instance.GetCurrentUser();
        
    }

    public async void Signup()
    {
        signupBtn.interactable = false;
        string email = emailField.text;
        string username = usernameField.text;
        string password = passwordField.text;
        string repeatPassword = repeatPasswordField.text;

        if (email == "" || email == null)
        {
            errorText.text = "No email";
            Debug.LogError("No email");
        }
        else if (username == "" || username == null)
        {
            errorText.text = "No username";
            Debug.LogError("No username");
        }
        else if (password == "" || password == null)
        {
            errorText.text = "No password";
            Debug.LogError("No password");
        }
        else if (password != repeatPassword)
        {
            errorText.text = "Password doesn't match";
            Debug.LogError("Password doesn't match");
        }
        else
        {
            try
            {
                var signupTask = await AuthManager.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);
                Debug.Log("REGISTERING...");
                string id = DateTime.UtcNow.ToString("yyMMddHHmmss");

                DocumentReference usersDoc = FirestoreManager.Instance.firestore.Collection("users").Document(id);

                Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                        { "email", email },
                        { "username", username },
                        { "history", new List<object>() },
                        { "role", "student" }
                    };

                await usersDoc.SetAsync(userData);
                Debug.Log("Registration Successful");
                //FirebaseAuth.DefaultInstance.SignOut();
            }
            catch (FirebaseException e) 
            {
                AuthError authError = (AuthError)e.ErrorCode;

                string failedMessage = "Signup failed: ";

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
                errorText.text = failedMessage;
                Debug.LogError(failedMessage);
            }
        }
        signupBtn.interactable = true;
    }
}
