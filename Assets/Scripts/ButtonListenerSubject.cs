using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListenerSubject : MonoBehaviour
{
    public Button english;
    public Button filipino;
    public Button science;

    private void Start()
    {
        english.onClick.AddListener(englishClicked);
        filipino.onClick.AddListener(filipinoClicked);
        science.onClick.AddListener(scienceClicked);
    }

    void englishClicked()
    {
        GameParamManager.subject = "English";
    }

    void filipinoClicked()
    {
        GameParamManager.subject = "Filipino";
    }

    void scienceClicked()
    {
        GameParamManager.subject = "Science";
    }
}
