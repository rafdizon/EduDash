using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI gameTitle;
    public TextMeshProUGUI gameCode;
    private float loadingTime; 
    void Start()
    {
        loadingTime = Random.Range(6.5f, 10f);
        gameTitle.text = FirestoreManager.Instance.gameTitle;
        gameCode.text = FirestoreManager.Instance.gameCode;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = elapsedTime / loadingTime; 
            yield return null;
        }

        SceneManager.LoadScene("GameModeHost"); 
    }
}
