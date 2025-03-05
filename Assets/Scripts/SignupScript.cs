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
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField repeatPassword;
    public TMP_InputField username;

    public FirebaseUser user;
    FirebaseFirestore db;

    private void Awake()
    {
        user = AuthManager.Instance.GetCurrentUser();
        
    }

    public void Signup()
    {
        StartCoroutine(SignupAsync(email.text, username.text, password.text, repeatPassword.text));
    }

    private IEnumerator SignupAsync(string email, string username, string password, string repeatPassword)
    {
        if (email == "" || email == null)
        {
            Debug.LogError("No email");
        }
        else if (username == "" || username == null)
        {
            Debug.LogError("No username");
        }
        else if (password == "" || password == null)
        {
            Debug.LogError("No password");
        }
        else if (password != repeatPassword)
        {
            Debug.LogError("Password doesn't match");
        }
        else
        {
            var signupTask = AuthManager.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => signupTask.IsCompleted);

            if(signupTask.Exception != null)
            {
                Debug.LogError(signupTask.Exception);
                FirebaseException firebaseException = signupTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

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
                Debug.LogError(failedMessage);
            }
            else
            {
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

                var addUserTask = usersDoc.SetAsync(userData);

                yield return new WaitUntil(() => addUserTask.IsCompleted);

                if (addUserTask.Exception != null)
                {
                    Debug.LogError($"Failed to add user to Firestore: {addUserTask.Exception}");
                }
                else
                {
                    Debug.Log("Registration Successful");
                }
            }
        }
    }
}
