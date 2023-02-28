using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Text_Money : MonoBehaviour
{
    public TextMeshProUGUI text_money;
    [SerializeField] Animator anim;

    private void OnValidate()
    {
        anim = GetComponent<Animator>();
    }

    private void Reset()
    {
        text_money = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        MoneyManager.I.SubscribeChange((money) =>
        {
            text_money.SetText($"{money}");

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)text_money.transform);
        });

        if (anim)
        {
            MoneyManager.I.SubscribeFailToBuy(() =>
            {
                anim.SetTrigger("Red");
            });
        }
    }
}
