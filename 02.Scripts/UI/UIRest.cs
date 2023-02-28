using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRest : UICanvas
{
    public TextMeshProUGUI Text_stage;

    public void SetStage(int stage)
    {
        Text_stage.SetText($"{stage}");
    }
}
