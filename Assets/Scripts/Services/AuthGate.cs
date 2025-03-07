using Firebase.Auth;
using System.Collections;
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
            StartCoroutine(WaitForUserDataThenLoadScene("MainMenu"));
        }
        else
        {
            SceneManager.LoadScene("LoginPage");
        }
    }
    private IEnumerator WaitForUserDataThenLoadScene(string sceneName)
    {
        yield return new WaitUntil(() => UserDataScript.Instance.userData != null); 
        SceneManager.LoadScene(sceneName);
    }
}