using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData
{
    public const string path_Money = "/money.json";
    public const string path_Player  = "/Player.json";
    public const string path_PlayerBuyCount = "/playerBuyCount.json";
    public const string path_Stage = "/Stage.json";

    public static void RemoveAllData()
    {
        File.Delete(Application.persistentDataPath + path_Money);
        File.Delete(Application.persistentDataPath + path_Player);
        File.Delete(Application.persistentDataPath + path_PlayerBuyCount);
        File.Delete(Application.persistentDataPath + path_Stage);

        Debug.Log("All Data Removed");
    }

    public static void SaveObj(object obj, string path)
    {
        string text = JsonUtility.ToJson(obj);
        File.WriteAllText(Application.persistentDataPath + path, text);
    }

    public static void SaveJson(string json, string path)
    {
        File.WriteAllText(Application.persistentDataPath + path, json);
    }

    public static T Load<T>(string path)
    {
        string text = File.ReadAllText(Application.persistentDataPath + path);
        return JsonUtility.FromJson<T>(text);
    }

    public static T LoadFromResources<T>(string path)
    {
        string textData = Resources.Load(path).ToString();
        return JsonUtility.FromJson<T>(textData);
    }

    public static string LoadJson(string path)
    {
        return File.ReadAllText(Application.persistentDataPath + path);
    }

    public static bool isFileExist(string path)
    {
        return File.Exists(Application.persistentDataPath + path);
    }
}