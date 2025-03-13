using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider progressBar; 
    private float loadingTime; 

    void Start()
    {
        loadingTime = Random.Range(6.5f, 10f);
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
