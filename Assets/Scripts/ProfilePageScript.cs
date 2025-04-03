using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilePageScript : MonoBehaviour
{
    public TextMeshProUGUI username;
    public TextMeshProUGUI userId;
    public GameObject cells;
    public GameObject row;
    public Transform table;
    //public TextMeshProUGUI 
    Dictionary<string, object> userData;
    private string userIdText;
    private async void Start()
    {
        userIdText = UserDataScript.Instance.userData["user_id"].ToString();
        userData = await FirestoreManager.Instance.GetUser();
        SetUpPage();
    }

    private void SetUpPage()
    {
        username.text = userData["username"].ToString();
        userId.text = userIdText;
        //List<Dictionary<string, string>> tableData = (List<Dictionary<string, string>>) userData["history"];

        List<object> rawTableData = (List<object>)userData["history"];
        List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();

        foreach (object row in rawTableData)
        {
            Dictionary<string, object> rawRow = (Dictionary<string, object>)row;
            Dictionary<string, string> convertedRow = new Dictionary<string, string>();

            string[] keyOrder = { "code", "title", "score" };

            foreach (string key in keyOrder)
            {
                if (rawRow.ContainsKey(key))
                {
                    convertedRow[key] = rawRow[key].ToString();
                }
            }

            tableData.Add(convertedRow);
        }

        foreach (Dictionary<string, string> dataRow in tableData) 
        { 
            GameObject newRow = Instantiate(row, table);

            foreach(string dataCell in dataRow.Values)
            {
                GameObject newCell = Instantiate(cells, newRow.transform);

                TMP_Text cellTMP = newCell.GetComponent<TMP_Text>();

                if (cellTMP != null) 
                {
                    cellTMP.text = dataCell;
                }
            }
        }
    }
}
