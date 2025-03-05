using System.IO;
using UnityEngine;

public class User_Info
{
    public int highScore;
    public int coins;
    public string userName;
}

public static class SaveSystem
{
    public static void SaveInfo(User_Info data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/userInfo.json", json);
    }

    public static User_Info LoadInfo()
    {
        string path = Application.persistentDataPath + "/userInfo.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<User_Info>(json);
        }
        return new User_Info();
    }
}
