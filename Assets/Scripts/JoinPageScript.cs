using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class JoinPageScript : MonoBehaviour
{
    [Space]
    [Header("Join")]
    public TMP_InputField gameCode;


    public async void JoinGame()
    {
        string code = gameCode.text.Trim();
        Dictionary<string, object> customQuiz = await FirestoreManager.Instance.GetQuiz(code);  

        if(customQuiz == null)
        {
            Debug.Log("No quiz found with code: " + code);
        }
        else
        {
            List<object> quizObjects = (List<object>)customQuiz["quiz"];
            QuizDataHolder.Instance.quizData = quizObjects
                .Select(obj => (Dictionary<string, object>)obj)
                .ToList();
            SceneManager.LoadScene("HostLoadingScreen");
        }

    }
}
