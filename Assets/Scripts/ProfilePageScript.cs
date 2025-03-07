using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilePageScript : MonoBehaviour
{
    public TextMeshProUGUI username;
    //public TextMeshProUGUI 
    Dictionary<string, object> userData;
    private void Start()
    {
        userData = UserDataScript.Instance.userData;
        SetUpPage();
    }

    private void SetUpPage()
    {
        username.text = userData["username"].ToString();
    }
}
