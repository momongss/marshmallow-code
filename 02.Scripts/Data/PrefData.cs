using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefData : SingletonScene<PrefData>
{
    public void Save(object obj, string key)
    {
        string text = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, text);
    }

    public T Load<T>(string key)
    {
        string text = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<T>(text);
    }

    public bool Has(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
