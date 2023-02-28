using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyPopulation : UICanvas
{
    public Button button_ad;
    public Button button_money;
    public TextMeshProUGUI Text_maxCountPrice;

    int[] priceList = new int[] {
        1000, 1000, 1000, 1000,
        1000, 1000, 1000, 1000,
        2000, 4000, 8000, 16000,
        32000, 64000, 128000 };

    int populationLimit = 40;

    public int GetPrice()
    {
        int maxPlayerCount = PlayerSquad.I.maxPlayerCount;

        if (maxPlayerCount == populationLimit)
        {
            button_money.GetComponentInChildren<TextMeshProUGUI>().SetText("Max");

            button_ad.interactable = false;
            button_money.interactable = false;

            return -1;
        } else if (maxPlayerCount > populationLimit)
        {
            Debug.LogError($"현재인구수 : {maxPlayerCount} > 한계인구수 :{populationLimit}");
            button_money.GetComponentInChildren<TextMeshProUGUI>().SetText("Max");

            button_ad.interactable = false;
            button_money.interactable = false;

            return -1;
        } else
        {
            int price;
            if (priceList.Length <= maxPlayerCount)
            {
                price = priceList[priceList.Length - 1];
            }
            else
            {
                price = priceList[maxPlayerCount];
            }

            Text_maxCountPrice.SetText($"$ {price}");

            return price;
        }
    }

    public override void Show()
    {
        base.Show();
        GetPrice();
    }

    void BuyPopulation()
    {
        PlayerSquad.I.AddPlayerCount();
    }

    public void BuyWithMoney()
    {
        int price = GetPrice();
        if (price == -1) return;

        bool success = MoneyManager.I.UseMoney(price);

        if (success)
        {
            BuyPopulation();
        }

        GetPrice();
    }

    public void BuyWithAds()
    {
        BuyPopulation();

        GetPrice();
    }
}
