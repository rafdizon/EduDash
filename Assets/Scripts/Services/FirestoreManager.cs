using Firebase.Firestore;
using Firebase;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Instance { get; private set; }
    public FirebaseFirestore firestore { get; private set; }

    private FirebaseUser user;

    public string gameCode;

    public string gameTitle;

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
        }
        InitializeFirestore();
    }
    private void Start()
    {
        user = AuthManager.Instance.GetCurrentUser();
    }

    private void InitializeFirestore()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firestore Initialized Successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }

    public async Task<Dictionary<string, object>> GetUser()
    {
        if (firestore == null)
        {
            Debug.LogError("Firestore is not initialized.");
            await Task.Delay(1000);
        }

        if (user == null)
        {
            Debug.LogError("No authenticated user found.");
            return null;
        }
        string email;
        email = user.Email;

        CollectionReference usersRef = firestore.Collection("users");
        Query query = usersRef.WhereEqualTo("email", email);

        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        if (querySnapshot.Documents.Count() > 0)
        {
            DocumentSnapshot userDoc = querySnapshot.Documents.FirstOrDefault();
            Dictionary<string, object> userData = userDoc.ToDictionary();
            userData["user_id"] = userDoc.Id;
            return userData;
        }
        else
        {
            Debug.Log("User not found.");
            //SceneManager.LoadScene("LoginPage");
            return null;
        }
    }

    public async Task<Dictionary<string, object>> GetQuiz(string code)
    {
        if (firestore == null)
        {
            Debug.LogError("Firestore is not initialized.");
            await Task.Delay(1000);
        }

        DocumentReference docRef = firestore.Collection("custom_quiz").Document(code);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        Debug.Log(snapshot.ToString());
        if (snapshot.Exists)
        {
            Dictionary<string, object> quizData = snapshot.ToDictionary();
            gameTitle = quizData["title"].ToString();
            return quizData;
        }
        else
        {
            Debug.LogWarning("Document not found.");
            return null;
        }
    }

    public async Task AddToHistory(int score)
    {
        if (firestore == null)
        {
            Debug.LogError("Firestore is not initialized.");
            await Task.Delay(1000);
        }

        Dictionary<string, object> userData = await GetUser();

        DocumentReference userRef = firestore.Collection("users").Document(userData["user_id"].ToString());
        Dictionary<string, object> userHistoryData = new Dictionary<string, object>
        {
            {"code" , gameCode},
            {"score", score},
            {"title", gameTitle }
        };

        Dictionary<string, object> updateUser = new Dictionary<string, object>
        {
            { "history", FieldValue.ArrayUnion(userHistoryData) },
        };

        DocumentReference quizRef = firestore.Collection("custom_quiz").Document(gameCode);
        Dictionary<string, object> playerHistoryData = new Dictionary<string, object>
        {
            {"user_id", userData["user_id"].ToString()},
            {"username", userData["username"].ToString() },
            {"score", score },
            {"date_played", DateTime.Now.ToString("MM-dd-yy hh:mm tt") }
        };

        Dictionary<string, object> updateQuiz = new Dictionary<string, object>
        {
            {"players", FieldValue.ArrayUnion(playerHistoryData) }
        };

        await userRef.UpdateAsync(updateUser);
        await quizRef.UpdateAsync(updateQuiz);
    }
}
