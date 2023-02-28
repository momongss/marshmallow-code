using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceManager : SingletonScene<PriceManager>
{
    int[] playerPrices;
    int[] playerMaxCountPrices;

    const string path = "prices";

    protected override void Awake()
    {
        base.Awake();

        Prices prices = JsonData.LoadFromResources<Prices>(path);

        playerPrices = prices.playerPrices;
        playerMaxCountPrices = prices.playerMaxCountPrices;
    }

    public int GetPlayerMaxCountPrices(int currPlayerMaxCount)
    {
        if (playerMaxCountPrices.Length <= currPlayerMaxCount)
        {
            return playerMaxCountPrices[playerMaxCountPrices.Length - 1];
        }

        return playerMaxCountPrices[currPlayerMaxCount];
    }

    public int GetPlayerPrice(int playerBuyCount)
    {
        if (playerPrices.Length <= playerBuyCount)
        {
            return playerPrices[playerPrices.Length - 1];
        }

        return playerPrices[playerBuyCount];
    }
}

[System.Serializable]
public class Prices
{
    public int[] playerPrices;
    public int[] playerMaxCountPrices;
}