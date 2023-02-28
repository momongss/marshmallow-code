using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : UICanvas
{
    public static UICharacterInfo I { get; private set; }

    public TextMeshProUGUI text_type;
    public TextMeshProUGUI text_attackPower;
    public TextMeshProUGUI text_attackSpeed;

    public TextMeshProUGUI text_Title;
    public TextMeshProUGUI text_hi;

    public Transform Panel_charater;

    protected override void Awake()
    {
        I = this;

        base.Awake();
    }

    public override void Hide()
    {
        base.Hide();

        ClearCharacterPanel();
    }

    public void ShowInfo(Player player)
    {
        Show();

        ClearCharacterPanel();

        Transform characterModel = Instantiate(
            player.transform.GetChild(0), 
            Panel_charater
            );

        Canvas[] canvases = characterModel.GetChild(0).GetComponentsInChildren<Canvas>();
        for (int i = 0; i < canvases.Length; ++i)
        {
            canvases[i].enabled = false;
        }

        //Talk talk = characterModel.GetChild(0).GetComponentInChildren<Talk>();
        //talk.GetComponent<Canvas>().enabled = true;
        //talk.Say(player.introduction, 100f);
        //print(player.introduction);

        text_hi.SetText(player.introduction);
        LayoutRebuilder.ForceRebuildLayoutImmediate(text_hi.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(text_hi.transform.parent.GetComponent<RectTransform>());

        characterModel.GetComponent<Animator>().enabled = false;
        
        if (player.type == Player.Type.Bazooka)
        {
            characterModel.localPosition = new Vector3(43f, -257f, -155f);
            characterModel.localRotation = Quaternion.Euler(new Vector3(18.4202919f, 198.909409f, 5.49838829f));
        } else
        {
            characterModel.localPosition = new Vector3(10f, -246f, -121f);
            characterModel.localRotation = Quaternion.Euler(new Vector3(18.4202919f, 198.909409f, 5.49838877f));
        }
        
        characterModel.localScale = new Vector3(70f, 70f, 70f);

        text_type.SetText(player.playerName);
        PlayerStat stat = PlayerStat.GetStat(player.type, player.level);

        text_attackPower.SetText($"{stat.damage}");
        text_attackSpeed.SetText($"{System.Math.Round(1 / stat.fireRate, 2)}");
    }

    public void ShowInfo(Player.Type type, int level)
    {
        Player player = PlayerSquad.I.GetPlayerPrefab(type, level);
        ShowInfo(player);
    }

    void ClearCharacterPanel()
    {
        for (int i = 0; i < Panel_charater.childCount; ++i)
        {
            Destroy(Panel_charater.GetChild(i).gameObject);
        }
    }
}
