using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataInt : MonoBehaviour
{
    public static DataInt I { get; private set; }

    public Type type;

    // enum Type 의 index와
    // dataPaths 의 index는 동기화 되어야한다.
    public enum Type
    {
        Money = 0,
        Exp = 1,
        LongGunLevel = 2,
        PistolLevel = 3,
        ShotGunLevel = 4
    }

    public static string[] dataPaths = new string[] {
        "/money.json",
        "/exp.json",
    };

    public TextMeshProUGUI[] subscribers;

    string dataPath;

    UnityEvent<int> dataEvent = new UnityEvent<int>();

    public int value { get; private set; }
    public int initValue = 0;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        I = this;
        dataPath = dataPaths[(int)type];

        if (subscribers == null) subscribers = new TextMeshProUGUI[0];

        if (JsonData.isFileExist(dataPath) == false) // 초기화
        {
            SaveValue(initValue);
        }
        else
        {
            string text = JsonData.LoadJson(dataPath);
            SetValue(int.Parse(text));
        }
    }

    public void Subscribe(UnityAction<int> action)
    {
        dataEvent.AddListener(action);
    }

    public void AddValue(int amount)
    {
        SaveValue(value + amount);
    }

    void SetValue(int _value)
    {
        value = _value;
        dataEvent.Invoke(value);

        string s_val = $"{value}";

        for (int i = 0; i < subscribers.Length; i++)
        {
            subscribers[i].text = s_val;
        }
    }

    public int GetValue()
    {
        return value;
    }

    public void SaveValue(int _value)
    {
        SetValue(_value);
        JsonData.SaveJson($"{value}", dataPath);
    }
}