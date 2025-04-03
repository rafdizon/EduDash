using UnityEngine;
using Firebase;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
public class FirebaseHandler : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase is ready.");
                SceneManager.LoadScene("InitialLoadingScene");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }
}
