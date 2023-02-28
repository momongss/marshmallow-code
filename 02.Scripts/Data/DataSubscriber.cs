using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataSubscriber : MonoBehaviour
{
    public DataInt dataInt;
    public TextMeshProUGUI text;

    private void Start()
    {
        text.text = $"{dataInt.value}";

        dataInt.Subscribe((int value) =>
        {
            text.text = $"{value}";
        });
    }
}
