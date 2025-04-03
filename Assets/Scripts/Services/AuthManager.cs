using UnityEngine;
using Firebase.Auth;
using System;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }
    public FirebaseAuth auth;

    public event Action<FirebaseUser> OnAuthStateChanged;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += HandleAuthStateChanged;
    }
    
    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        OnAuthStateChanged?.Invoke(auth.CurrentUser);
    }

    public FirebaseUser GetCurrentUser()
    {
        return auth.CurrentUser;
    }
    public void NotifyAUthStateChanged()
    {
        OnAuthStateChanged?.Invoke(auth.CurrentUser);
    }
}
