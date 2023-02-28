using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataIntSimple
{
    string dataPath;

    public int value { get; private set; }

    public DataIntSimple(string _dataPath, int initValue = 0)
    {
        dataPath = _dataPath;

        if (JsonData.isFileExist(dataPath) == false) // √ ±‚»≠
        {
            SaveValue(initValue);
        }
        else
        {
            value = int.Parse(JsonData.LoadJson(dataPath));
        }
    }

    public void AddValue(int amount)
    {
        SaveValue(value + amount);
    }

    public int GetValue()
    {
        return value;
    }

    public void SaveValue(int _value)
    {
        value = _value;
        JsonData.SaveJson($"{value}", dataPath);
    }
}
