using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class BattleSceneManager : SingletonScene<BattleSceneManager>
{
    [SerializeField] UIBattleEnd UI_battleEnd;
    [SerializeField] UICanvas UI_Battle;

    [SerializeField] RectTransform box_point;
    TextMeshProUGUI text_point;

    public int stage;
    public int point = 0;

    UnityEvent<int> pointChangeEvent = new UnityEvent<int>();

    public bool isDev = true;

    float startTime;
    float endTime;

    [SerializeField] TextMeshProUGUI text_stage;

    protected override void Awake()
    {
        base.Awake();
        print(Application.persistentDataPath);

        // text_point = box_point.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        LoadtStage();
        StartBattle();
    }

    string stagePath = "/Stage.json";

    void LoadtStage()
    {
        if (isDev)
        {
            return;
        }

        if (JsonData.isFileExist(stagePath))
        {
            stage = Int32.Parse(JsonData.LoadJson(stagePath));
        }
        else
        {
            stage = 0;
            JsonData.SaveJson($"{stage}", stagePath);
        }
    }

    void SetStage(int _stage)
    {
        stage = _stage;
        
        JsonData.SaveJson($"{stage}", stagePath);
    }

    public void SubscribePoint(UnityAction<int> action)
    {
        pointChangeEvent.AddListener(action);
    }

    public void SetPoint(int _point)
    {
        point = _point;

        // text_point.SetText($"{point}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(box_point);

        pointChangeEvent.Invoke(point);
    }

    public void StartBattle()
    {
        text_stage.SetText($"STAGE {stage}");

        UI_battleEnd.Hide();
        UI_Battle.Show();

        SetPoint(0);

        startTime = Time.time;

        PlayerSquad.I.StartBattle();
        EnemySquadManager.I.StartBattle(stage);
    }

    public void EndBattle(bool isWin)
    {
        float totalDamage = EnemySquadManager.I.EndBattle(isWin);
        endTime = Time.time;

        StartCoroutine(_EndBattle(isWin, totalDamage));
    }

    IEnumerator _EndBattle(bool isWin, float totalDamage)
    {
        yield return new WaitForSeconds(2f);

        this.isWin = isWin;

        if (isWin)
        {
            Debug.Log("Mission Complete!! Total Point : " + point);

            UI_battleEnd.Show(true, totalDamage, endTime - startTime);
        }

        else
        {
            Debug.Log("Fail Mission Total Point : " + point);

            UI_battleEnd.Show(false, totalDamage, endTime - startTime);
        }
    }

    bool isWin;

    // 외부 버튼 참조
    public void GetReward()
    {
        if (isWin)
        {
            SetStage(stage + 1);
        }

        GameManager.I.OnBattleEnd(isWin, point);
        MoneyManager.I.AddMoney(point);

        UI_battleEnd.Hide();

        ApplovinManager.I.ShowInterstitialAds();

        SceneManager.LoadScene("PlayScene");
    }

    public void GetAdReward(int reward)
    {
        if (isWin)
        {
            SetStage(stage + 1);
        }

        MoneyManager.I.AddMoney(reward);

        UI_battleEnd.Hide();

        SceneManager.LoadScene("PlayScene");
    }
}
