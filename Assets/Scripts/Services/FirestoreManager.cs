using Firebase.Firestore;
using Firebase;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Linq;
using System.Collections.Generic;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Instance { get; private set; }
    public FirebaseFirestore firestore { get; private set; }

    private FirebaseUser user;

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
            //Debug.Log(userDoc.ToDictionary().ToString());
            return userDoc.ToDictionary();
        }
        else
        {
            Debug.Log("User not found.");
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
            return snapshot.ToDictionary();
        }
        else
        {
            Debug.LogWarning("Document not found.");
            return null;
        }
    }
}
