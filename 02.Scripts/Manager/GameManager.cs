using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonScene<GameManager>
{
    int stage;
    string stagePath = "/Stage.json";

    public bool isWin = true;
    public int battlePoint = 0;

    public void OnBattleEnd(bool isWin, int battlePoint)
    {
        this.isWin = isWin;
        this.battlePoint = battlePoint;
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public int LoadtStage()
    {
        if (JsonData.isFileExist(stagePath))
        {
            stage = Utils.ToInt(JsonData.LoadJson(stagePath));
        }
        else
        {
            stage = 0;
            JsonData.SaveJson($"{stage}", stagePath);
        }

        return stage;
    }
}
