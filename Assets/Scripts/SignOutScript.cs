using Firebase.Auth;
using System.Collections;
using UnityEngine;

public class SignOutScript : MonoBehaviour
{
    [Space]
    [Header("Signout")]
    public FirebaseUser user;
    private void Awake()
    {
        user = AuthManager.Instance.GetCurrentUser();
    }
    public void SignOut()
    {
        if(AuthManager.Instance.auth != null && user != null)
        {
            AuthManager.Instance.auth.SignOut();
        }
    }
}
