using UnityEngine;
using UnityEngine.UI;

public class ButtonListener_Difficulty : MonoBehaviour
{
    public Button easy;
    public Button medium;
    public Button hard;

    private void Start()
    {
        easy.onClick.AddListener(easyClicked);
        medium.onClick.AddListener(mediumClicked);
        hard.onClick.AddListener(hardClicked);
    }

    void easyClicked()
    {
        GameParamManager.difficulty = "Easy";
    }

    void mediumClicked() 
    {
        GameParamManager.difficulty = "Medium";
    }

    void hardClicked()
    {
        GameParamManager.difficulty = "Hard";
    }
}
