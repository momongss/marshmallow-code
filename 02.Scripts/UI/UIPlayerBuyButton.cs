using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerBuyButton : MonoBehaviour
{  
    [SerializeField] TextMeshProUGUI text_price;

    private void Start()
    {
        SetPriceText();
    }

    public void BuyPlayer()
    {
        PlayerSquad.I.BuyPlayer();

        SetPriceText();
    }

    void SetPriceText()
    {
        text_price.SetText($"{ShopManager.I.GetPlayerPrice()}");
    }
}
