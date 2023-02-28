using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRewardPopUp : SingletonScene<UIRewardPopUp>
{
    public TextMeshProUGUI text_reward;
    int reward = -40000;

    public Vector2 showPosition;
    public Vector2 hidePosition;

    public void Show()
    {
        reward = Mathf.Max(
            ShopManager.I.GetPlayerPrice() * Random.Range(8, 12),
            GameManager.I.battlePoint * 5
            );

        if (GameManager.I.LoadtStage() > 15)
        {
            reward *= 2;
        }

        StartCoroutine(_Show());
    }

    IEnumerator _Show()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = hidePosition;

        gameObject.SetActive(true);
        text_reward.SetText($"{string.Format("{0:#,0}", reward)}");
        yield return new WaitForSeconds(0.2f);

        rectTransform.DOAnchorPos(showPosition, 0.5f);
    }

    public void Hide()
    {
        StartCoroutine(_Hide());
    }

    IEnumerator _Hide()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(hidePosition, 0.5f);
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    public void ShowRewardAd()
    {
        ApplovinManager.I.ShowRewardAd(() =>
        {
            if (reward <= 0)
            {
                Debug.LogError($"Reward ºñÁ¤»ó {reward}");
                reward = 40000;
            }

            MoneyManager.I.AddMoney(reward);
        });
        
        Hide();
    }
}
