using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingletonScene<ShopManager>
{
    public int playerBuyCount = 1;

    const string path = "/playerBuyCount.json";

    bool isInitialized = false;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (isInitialized) return;

        isInitialized = true;
        if (JsonData.isFileExist(path))
        {
            playerBuyCount = Utils.ToInt(JsonData.LoadJson(path));
        }
        else
        {
            playerBuyCount = 0;
            JsonData.SaveJson($"{playerBuyCount}", path);
        }
    }

    public bool BuyPlayer()
    {
        bool buySuccess = MoneyManager.I.UseMoney(GetPlayerPrice());

        if (buySuccess)
        {
            ++playerBuyCount;
            JsonData.SaveJson($"{playerBuyCount}", path);
        }

        return buySuccess;
    }

    public int GetPlayerPrice()
    {
        Init();
        return PriceManager.I.GetPlayerPrice(playerBuyCount);
    }
}
