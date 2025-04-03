using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Collections;
using Firebase;
using TMPro;
using Firebase.Firestore;
using System.Threading.Tasks;

public class LoginScript : MonoBehaviour
{
    [Space]
    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public TextMeshProUGUI errorText;
    public Button revealText;
    public Button loginBtn;
    public FirebaseUser user;

    private bool isPasswordVisible = false;

    private void Awake()
    {
        user = AuthManager.Instance.GetCurrentUser();
    }

    public void RevealText()
    {
        isPasswordVisible = !isPasswordVisible;
        password.contentType = isPasswordVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        password.ForceLabelUpdate();
    }
    public async void Login()
    {
        loginBtn.interactable = false;
        try
        {
            var loginTask = await AuthManager.Instance.auth.SignInWithEmailAndPasswordAsync(email.text, password.text);
            this.user = loginTask.User;
            Debug.Log("Login Successful");

            await LoadUserData(this.user.UserId);
            AuthManager.Instance.NotifyAUthStateChanged();
        }
        catch (FirebaseException e)
        {
            AuthError errorCode = (AuthError)e.ErrorCode;
            string failedMessage = "Login failed: ";

            switch (errorCode)
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
            Debug.Log(errorCode.ToString());
        }
        finally
        {
            loginBtn.interactable = true;
        }
    }
    private async Task LoadUserData(string userId)
    {
        DocumentReference docRef = FirestoreManager.Instance.firestore.Collection("users").Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            UserDataScript.Instance.userData = snapshot.ToDictionary();
            Debug.Log("User data loaded");
        }
        else
        {
            Debug.LogError("User data not found");
        }
    }

    //private IEnumerator LoginAsync(string email, string password)
    //{
    //    var loginTask = AuthManager.Instance.auth.SignInWithEmailAndPasswordAsync(email, password);

    //    yield return new WaitUntil(() =>  loginTask.IsCompleted);

    //    if(loginTask.Exception != null)
    //    {
    //        Debug.LogError(loginTask.Exception);
    //        FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
    //        AuthError authError = (AuthError)firebaseException.ErrorCode;

    //        string failedMessage = "Login failed: ";

    //        switch (authError) 
    //        {
    //            case AuthError.InvalidEmail:
    //                failedMessage += "Invalid email";
    //                break;
    //            case AuthError.WrongPassword:
    //                failedMessage += "Wrong password";
    //                break;
    //            case AuthError.UserNotFound:
    //                failedMessage += "User not found, signup first";
    //                break;
    //            case AuthError.MissingEmail:
    //                failedMessage += "Enter email";
    //                break;
    //            case AuthError.MissingPassword:
    //                failedMessage += "Enter password";
    //                break;
    //            default:
    //                failedMessage += "Unknown error, contact support";
    //                break;
    //        }
    //        errorText.text = failedMessage;
    //        Debug.Log(failedMessage);
    //    }
    //    else
    //    {
    //        user = loginTask.Result.User;
    //        AuthManager.Instance.NotifyAUthStateChanged();
    //    }
    //}
}
