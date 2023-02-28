using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBattleSelection : UICanvas
{
    protected override void Awake()
    {
        base.Awake();

        Hide();
    }

    public void StartStage(int stage)
    {
        SceneManager.LoadScene($"Stage {stage}");
    }
}
