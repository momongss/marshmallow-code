using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillFeverManager : SingletonScene<SkillFeverManager>
{
    [SerializeField] FeverData[] feverInfoList = new FeverData[] {
        new FeverData(2f, 2f, 1.3f, 12f, 35f),
        new FeverData(2.5f, 2f, 1.3f, 7f, 28f),
        new FeverData(3f, 2f, 1.3f, 8f, 28f),
        new FeverData(4f, 2f, 1.3f, 9f, 26f),
        new FeverData(5f, 3f, 1.3f, 10f, 24f),
    };

    [SerializeField] Button button_fever;
    [SerializeField] Image image_coolTime;

    int feverLevel = 0;

    const string path = "/fever.json";

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (JsonData.isFileExist(path))
        {
            feverLevel = Utils.ToInt(JsonData.LoadJson(path));
        }
        else
        {
            feverLevel = 0;
            JsonData.SaveJson($"{feverLevel}", path);
        }
    }

    public void StartFever()
    {
        PlayerSquad.I.StartFever(feverInfoList[feverLevel]);

        button_fever.interactable = false;

        image_coolTime.fillAmount = 1f;

        StartCoroutine(EndFever());
    }

    IEnumerator EndFever()
    {
        yield return new WaitForSeconds(feverInfoList[feverLevel].feverTime);

        print("End");
        PlayerSquad.I.EndFever();
    }

    private void Update()
    {
        if (image_coolTime.fillAmount > 0f)
        {
            image_coolTime.fillAmount -= Time.deltaTime / feverInfoList[feverLevel].coolTime;
            if (image_coolTime.fillAmount <= 0f)
            {
                button_fever.interactable = true;
            }
        }
    }

    public class FeverData
    {
        public float damage;
        public float fireRate;
        public float moveSpeed;
        public float feverTime;
        public float coolTime;

        public FeverData(float damage, float fireRate, float moveSpeed, float feverTime, float coolTime)
        {
            this.damage = damage;
            this.fireRate = fireRate;
            this.moveSpeed = moveSpeed;
            this.feverTime = feverTime;
            this.coolTime = coolTime;
        }
    }
}