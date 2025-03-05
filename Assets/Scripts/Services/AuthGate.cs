using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthGate : MonoBehaviour
{
    private void Start()
    {
        AuthManager.Instance.OnAuthStateChanged += HandleAuthStateChanged;
        HandleAuthStateChanged(AuthManager.Instance.GetCurrentUser());
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void HandleAuthStateChanged(FirebaseUser user)
    {
        if (user != null) 
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("LoginPage");
        }
    }
}