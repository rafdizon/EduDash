using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizDataHolder : MonoBehaviour
{
    public static QuizDataHolder Instance;
    public List<Dictionary<string, object>> quizData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    //public void ProcessQuiz(List<Dictionary<string, object>> rawQuiz)
    //{
    //    if (rawQuiz.TryGetValue("quiz", out object quizField))
    //    {
    //        // Firestore returns arrays as List<object>
    //        List<object> quizArray = quizField as List<object>;
    //        if (quizArray != null)
    //        {
    //            questionsList = quizArray
    //                .Select(item => item as Dictionary<string, object>)
    //                .Where(dict => dict != null)
    //                .ToList();

    //            Debug.Log($"Successfully retrieved {questionsList.Count} questions.");
    //        }
    //        else
    //        {
    //            Debug.LogError("The quiz field is not an array.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Quiz field not found in the Firestore data.");
    //    }
    //}
}
