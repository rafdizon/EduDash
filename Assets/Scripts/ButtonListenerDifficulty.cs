using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener_Difficulty : MonoBehaviour
{
    public Button option1;
    public Button option2;
    public Button option3;

    private TextMeshProUGUI option1Text;
    private TextMeshProUGUI option2Text;
    private TextMeshProUGUI option3Text;
    private void Start()
    {
        
        option1.onClick.AddListener(option1Clicked);
        option2.onClick.AddListener(option2Clicked);
        option3.onClick.AddListener(option3Clicked);

        option1Text = option1.GetComponentInChildren<TextMeshProUGUI>();
        option2Text = option2.GetComponentInChildren<TextMeshProUGUI>();
        option3Text = option3.GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log(GameParamManager.subject);

        if (GameParamManager.subject == "Science")
        {
            option1Text.text = "Living Things and Their Environment";
            option2Text.fontSize = 40;

            option2Text.text = "Earth and Space";
            option2Text.fontSize = 50;

            option3Text.text = "Matter";
            option3Text.fontSize = 70;
        }
        else if (GameParamManager.subject == "English")
        {
            option1Text.text = "Grammar";
            option1Text.fontSize = 70;

            option2Text.text = "Spell";
            option2Text.fontSize = 70;

            option3Text.text = "Vocabulary";
            option3Text.fontSize = 60;
        }
        else if (GameParamManager.subject == "Filipino")
        {
            option1Text.text = "Masining na Pagpapahayag";
            option1Text.fontSize = 40;

            option2Text.text = "Pagsulat ng Sanaysay";
            option2Text.fontSize = 40;

            option3Text.text = "Panitikan ng Pilipinas";
            option3Text.fontSize = 40;
        }
    }

    void option1Clicked()
    {
        if(GameParamManager.subject == "Science")
        {
            GameParamManager.topic = "Living Things and Their Environment";
        }
        else if (GameParamManager.subject == "English")
        {
            GameParamManager.topic = "Grammar";
        }
        else if (GameParamManager.subject == "Filipino")
        {
            GameParamManager.topic = "Masining na Pagpapahayag";
        }
    }

    void option2Clicked() 
    {
        if (GameParamManager.subject == "Science")
        {
            GameParamManager.topic = "Earth and Space";
        }
        else if (GameParamManager.subject == "English")
        {
            GameParamManager.topic = "Spell";
        }
        else if (GameParamManager.subject == "Filipino")
        {
            GameParamManager.topic = "Pagsulat ng Sanaysay";
        }
    }

    void option3Clicked()
    {
        if (GameParamManager.subject == "Science")
        {
            GameParamManager.topic = "Matter";
        }
        else if (GameParamManager.subject == "English")
        {
            GameParamManager.topic = "Vocabulary";
        }
        else if (GameParamManager.subject == "Filipino")
        {
            GameParamManager.topic = "Panitikan ng Pilipinas";
        }
    }
}
