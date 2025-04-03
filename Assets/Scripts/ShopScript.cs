using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class ShopScript : MonoBehaviour
{
    public TextMeshProUGUI totalCoinsText;

    public Button item1;
    public Button item2;
    public Button item3;
    public Button item4;

    public GameObject confirmationPanel;
    public Button yesBtn;
    public Button noBtn;

    private Color selectedColor;
    private string selectedHex;
    private const int colorCost = 500;

    private Dictionary<Button, string> colorMap = new Dictionary<Button, string>();

    void Start()
    {
        colorMap[item1] = "A1ADFF";
        colorMap[item2] = "FFA1DD";
        colorMap[item3] = "A7A7A7";
        colorMap[item4] = "FF262B";

        foreach (var entry in colorMap)
        {
            entry.Key.onClick.AddListener(() => ShowConfirmationPanel(entry.Value));
        }

        yesBtn.onClick.AddListener(PurchaseOrEquipColor);
        noBtn.onClick.AddListener(() => confirmationPanel.SetActive(false));

        confirmationPanel.SetActive(false);
        UpdateUI();
    }

    void ShowConfirmationPanel(string hexColor)
    {
        if (!hexColor.StartsWith("#"))
        {
            hexColor = "#" + hexColor;
        }

        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out selectedColor))
        {
            selectedHex = hexColor;

            User_Info info = SaveSystem.LoadInfo();
            if (info.purchasedColors.Contains(selectedHex))
            {
                PurchaseOrEquipColor();
                return;
            }

            confirmationPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid color code: " + hexColor);
        }
    }

    void PurchaseOrEquipColor()
    {
        User_Info info = SaveSystem.LoadInfo();

        if (info.purchasedColors.Contains(selectedHex))
        {
            info.avatarColor = selectedHex;
            Debug.Log("Color equipped: " + selectedHex);
        }
        else if (info.coins >= colorCost)
        {
            info.coins -= colorCost;
            info.purchasedColors.Add(selectedHex);
            info.avatarColor = selectedHex;
            Debug.Log("Color purchased & equipped: " + selectedHex);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }

        SaveSystem.SaveInfo(info);
        confirmationPanel.SetActive(false);
        UpdateUI();
    }

    void UpdateUI()
    {
        User_Info info = SaveSystem.LoadInfo();
        totalCoinsText.text = info.coins.ToString();

        foreach (var entry in colorMap)
        {
            string hexColor = "#" + entry.Value;
            Button button = entry.Key;

            if (info.purchasedColors.Contains(hexColor))
            {
                //button.GetComponentInChildren<Image>().gameObject.SetActive(false);
                button.GetComponentInChildren<TMP_Text>().text = (info.avatarColor == hexColor) ? "Equipped" : "Equip";
            }
            else
            {
                button.GetComponentInChildren<TMP_Text>().text = "Buy (" + colorCost + ")";
            }
        }
    }
}
