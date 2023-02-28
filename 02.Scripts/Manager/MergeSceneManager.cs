using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MergeSceneManager : SingletonScene<MergeSceneManager>
{
    [SerializeField] UIRest UI_Rest;

    private void Start()
    {
        StartRest();
    }

    public void StartRest()
    {
        int stage = GameManager.I.LoadtStage();
        UI_Rest.SetStage(stage);

        PlayerSquad.I.StartMerge();

        bool isWinBattle = GameManager.I.isWin;
        if (isWinBattle)
        {
            if (ApplovinManager.I.isRewardAdReady())
            {
                UIRewardPopUp.I.Show();
            }
        }
        else
        {
            if (ApplovinManager.I.isRewardAdReady_lost())
            {
                UIRewardPopUp.I.Show();
            }          
        }
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
