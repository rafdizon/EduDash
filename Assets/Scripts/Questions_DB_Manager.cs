using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class Questions_DB_Manager : MonoBehaviour
{
    private SQLiteConnection dbConnection;

    private void OnDestroy()
    {
        dbConnection.Close();
    }

    public delegate void OnQuestionsLoaded(List<Questions> questions);

    public void InitializeDatabase(string subject, string topic, OnQuestionsLoaded callback)
    {
        StartCoroutine(LoadDatabaseCoroutine(subject, topic, callback));
    }

    private IEnumerator LoadDatabaseCoroutine(string subject, string topic, OnQuestionsLoaded callback)
    {
        string dbName = "Questions_New.db";
        //string dbPath = Path.Combine(Application.persistentDataPath, dbName);
        string dbPath = Path.Combine(Application.streamingAssetsPath, dbName);

        if (!File.Exists(dbPath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);

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

        dbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
        Debug.Log("Database opened at: " + dbPath);

        if (dbConnection == null)
        {
            Debug.LogError("Database connection is null.");
            yield break; 
        }

        var questions = dbConnection.Query<Questions>($"SELECT * FROM Questions WHERE Subject='{subject}' AND Topic='{topic}'");

        Debug.Log("Questions retrieved: " + questions.Count);

        callback(questions);
    }

    public void GetQuestions(string subject, string topic, OnQuestionsLoaded callback)
    {
        InitializeDatabase(subject, topic, callback);
    }
}

public class Questions
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Topic { get; set; }
    public string Question { get; set; }
    public string Choice_A { get; set; }
    public string Choice_B { get; set; }
    public string Choice_C { get; set; }
    public string Correct_Answer { get; set; }
}