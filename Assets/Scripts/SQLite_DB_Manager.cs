using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;


public class SQLite_DB_Manager : MonoBehaviour
{
    private SQLiteConnection dbConnection;

    private void OnDestroy()
    {
        dbConnection.Close();
    }

    // Callback for when questions are loaded
    public delegate void OnQuestionsLoaded(List<Questions> questions);

    public void InitializeDatabase(string subject, string difficulty, OnQuestionsLoaded callback)
    {
        StartCoroutine(LoadDatabaseCoroutine(subject, difficulty, callback));
    }

    private IEnumerator LoadDatabaseCoroutine(string subject, string difficulty, OnQuestionsLoaded callback)
    {
        string dbName = "Questions.db"; // your database name
        string dbPath = Path.Combine(Application.persistentDataPath, dbName);

        if (!File.Exists(dbPath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);

            // For Android or other platforms that require special handling
#if UNITY_ANDROID
            UnityWebRequest loadDb = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + dbName);
            yield return loadDb.SendWebRequest(); // Wait for the request to finish

            if (loadDb.result == UnityWebRequest.Result.Success && loadDb.downloadHandler != null)
            {
                // Only write the file if the data is not null
                File.WriteAllBytes(dbPath, loadDb.downloadHandler.data);
                Debug.Log("Database copied to: " + dbPath);
            }
            else
            {
                // Log error if something went wrong
                Debug.LogError("Failed to load database from StreamingAssets: " + loadDb.error);
            }
#else
            File.Copy(sourcePath, dbPath);
#endif

            Debug.Log("Database copied to: " + dbPath);
        }

        // Open database connection
        dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
        Debug.Log("Database opened at: " + dbPath);

        if (dbConnection == null)
        {
            Debug.LogError("Database connection is null.");
            yield break; // Exit if the connection failed
        }

        var questions = dbConnection.Query<Questions>($"SELECT * FROM Questions WHERE Subject='{subject}' AND Difficulty='{difficulty}'");

        Debug.Log("Questions retrieved: " + questions.Count);

        // Call the callback after loading the questions
        callback(questions);
    }

    public void GetQuestions(string subject, string difficulty, OnQuestionsLoaded callback)
    {
        // Start the coroutine to load the database and return the result via callback
        InitializeDatabase(subject, difficulty, callback);
    }
}
    /*
    private SQLiteConnection dbConnection;
    private void OnDestroy()
    {
        dbConnection.Close();    
    }

    public List<Questions> GetQuestions(string subject, string difficulty)
    {
        string dbName = "Questions.db"; // your database name
        string dbPath = Path.Combine(Application.persistentDataPath, dbName);

        if (!File.Exists(dbPath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);

            // For Android or other platforms that require special handling
#if UNITY_ANDROID
            var loadDb = new UnityWebRequest("jar:file://" + Application.dataPath + "!/assets/" + dbName);
            while (!loadDb.isDone) { } // Be careful with while loops in production code
            File.WriteAllBytes(dbPath, loadDb.downloadHandler.data);
#else
        File.Copy(sourcePath, dbPath);
#endif

            Debug.Log("Database copied to: " + dbPath);
        }

        // Open database connection
        dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
        Debug.Log("Database opened at: " + dbPath);
        //Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        //var dbPath = Path.Combine(Application.persistentDataPath, "Questions.db");
        //dbPath = dbPath.Replace("\\", "/");
        //Debug.Log(dbPath);
        //dbConnection = new SQLiteConnection(dbPath,SQLiteOpenFlags.ReadWrite);
        if (dbConnection == null)
        {
            Debug.LogError("Database connection is null.");
            return null;
        }
        var questions = dbConnection.Query<Questions>($"SELECT * FROM Questions WHERE Subject='{subject}' AND Difficulty='{difficulty}'");

        //Debug.Log(questions);
        return questions;
    }
}
*/
public class Questions
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Difficulty { get; set; }
    public string Question { get; set; }
    public string Choices_A { get; set; }
    public string Choices_B { get; set; }
    public string Choices_C { get; set; }
    public string Correct_Answer { get; set; }
}