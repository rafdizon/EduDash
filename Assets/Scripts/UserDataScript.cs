using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UserDataScript : MonoBehaviour
{
    public static UserDataScript Instance {  get; private set; }
    public Slider progressBar;
    public Dictionary<string, object> userData;

    private void Awake()
    {
        progressBar.maxValue = 100;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(GetProgress());
    }
    IEnumerator GetProgress()
    {
        Task task = InitUserData();

        while(!task.IsCompleted)
        {
            progressBar.value += 1f;
            yield return new WaitForSeconds(0.1f);
        }

        progressBar.value = 100f;
    }
    private async Task InitUserData()
    {
        userData = await FirestoreManager.Instance.GetUser();
    }
}
