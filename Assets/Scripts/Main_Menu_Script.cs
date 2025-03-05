using TMPro;
using UnityEngine;

public class Main_Menu_Script : MonoBehaviour
{
    public TextMeshProUGUI totalCoinsText;

    private void Update()
    {
        User_Info userInfo = SaveSystem.LoadInfo();
        totalCoinsText.text = userInfo.coins.ToString();
    }
}
