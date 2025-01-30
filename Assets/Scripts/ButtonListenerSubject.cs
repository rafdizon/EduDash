using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListenerSubject : MonoBehaviour
{
    public Button english;
    public Button filipino;
    public Button math;

    private void Start()
    {
        english.onClick.AddListener(englishClicked);
        filipino.onClick.AddListener(filipinoClicked);
        math.onClick.AddListener(mathClicked);
    }

    void englishClicked()
    {
        GameParamManager.subject = "English";
    }

    void filipinoClicked()
    {
        GameParamManager.subject = "Filipino";
    }

    void mathClicked()
    {
        GameParamManager.subject = "Math";
    }
}
