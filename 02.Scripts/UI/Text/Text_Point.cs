using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Point : MonoBehaviour
{
    public TextMeshProUGUI text_point;

    private void Reset()
    {
        text_point = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        BattleSceneManager.I.SubscribePoint((point) =>
        {
            text_point.SetText($"$ {point}");
        });
    }
}
