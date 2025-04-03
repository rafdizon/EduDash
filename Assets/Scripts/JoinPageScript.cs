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
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    private void Start()
    {
        warningPanel.SetActive(false);
    }
    public async void JoinGame()
    {
        string code = gameCode.text.Trim();
        Dictionary<string, object> customQuiz = await FirestoreManager.Instance.GetQuiz(code);

        if (customQuiz == null)
        {
            StartCoroutine(showWarning("No game found with this code!"));
            Debug.Log("No quiz found with code: " + code);
        }
        else
        {
            if (customQuiz.ContainsKey("players"))
            {
                List<object> playersList = customQuiz["players"] as List<object>;
                string currentUserId = UserDataScript.Instance.userData["user_id"].ToString();

                if (playersList != null && playersList.Any(player => ((Dictionary<string, object>)player)["user_id"].ToString() == currentUserId))
                {
                    Debug.Log("User has already played the game.");
                    StartCoroutine(showWarning("You already played this game!"));
                    return;
                }
            }
            List<object> quizObjects = (List<object>)customQuiz["quiz"];
            QuizDataHolder.Instance.quizData = quizObjects
                .Select(obj => (Dictionary<string, object>)obj)
                .ToList();
            FirestoreManager.Instance.gameCode = code;
            SceneManager.LoadScene("HostLoadingScreen");
        }
    }
    private IEnumerator showWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        warningPanel.SetActive(false);
    }
}
