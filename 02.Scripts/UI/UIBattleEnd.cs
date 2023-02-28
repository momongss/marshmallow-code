using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleEnd : UICanvas
{
    [SerializeField] TextMeshProUGUI Text_Win;
    [SerializeField] TextMeshProUGUI Text_Lose;

    [SerializeField] TextMeshProUGUI Text_totalDamage;
    [SerializeField] TextMeshProUGUI Text_totalTime;

    protected override void Awake()
    {
        base.Awake();

        Hide();
    }

    public void Show(bool isWin, float totalDamage, float battleTime)
    {
        Show();

        Text_totalDamage.SetText($"{string.Format("{0:#,###0}", (int)totalDamage)}");
        Text_totalTime.SetText($"{(int)battleTime} s");

        if (isWin)
        {
            Text_Win.enabled = true;
            Text_Lose.enabled = false;
        } else
        {
            Text_Win.enabled = false;
            Text_Lose.enabled = true;
        }
    }
}
