using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : SingletonGame<MoneyManager>
{
    const string path = "/money.json";

    UnityEvent<ulong> changeEvent = new UnityEvent<ulong>();
    UnityEvent FailBuyEvent = new UnityEvent();

    ulong money;

    protected override void Awake()
    {
        base.Awake();

        if (JsonData.isFileExist(path))
        {
            money = System.Convert.ToUInt64(JsonData.LoadJson(path));
        } else
        {
            money = 0;
            Save();
        }
    }

    public bool UseMoney(int amount)
    {
        ulong u_amount = (ulong)amount;

        if (money >= u_amount)
        {
            money -= u_amount;

            Save();

            changeEvent.Invoke(money);
            return true;
        }
        else
        {
            FailBuyEvent.Invoke();
            return false;
        }
    }

    void Save()
    {
        JsonData.SaveJson($"{money}", path);
    }

    public void AddMoney(int amount)
    {
        ulong u_amount = (ulong)amount;

        money += u_amount;
        Save();

        changeEvent.Invoke(money);
    }

    public void SubscribeChange(UnityAction<ulong> action)
    {
        changeEvent.AddListener(action);
        action(money);
    }

    public void SubscribeFailToBuy(UnityAction action)
    {
        FailBuyEvent.AddListener(action);
    }
}