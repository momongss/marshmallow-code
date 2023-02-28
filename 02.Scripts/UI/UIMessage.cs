using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIMessage : UICanvas
{
    public static UIMessage I { get; private set; }

    public TextMeshProUGUI text_Messsage;

    protected override void Awake()
    {
        I = this;
        base.Awake();

        canvas.enabled = false;

        Hide();
    }

    public void ShowMessage(string msg, float showTime = 1.5f)
    {
        StartCoroutine(_Show(msg, showTime));
    }

    IEnumerator _Show(string msg, float showTime)
    {
        text_Messsage.text = msg;
        Show();

        yield return new WaitForSeconds(showTime);

        Hide();
    }

    public override void Show()
    {
        base.Show();

        if (showSequence != null)
        {
            showSequence.Kill();
            showSequence = null;
        }

        if (hideSequence != null)
        {
            hideSequence.Kill();
            hideSequence = null;
        }

        showSequence = TweenAnimations.UI_Scaling_Show(panel, originScale);
    }

    Sequence showSequence = null;
    Sequence hideSequence = null;

    public override void Hide()
    {
        hideSequence = TweenAnimations.ScaleToZero(panel, () =>
        {
            canvas.enabled = false;

            showSequence = null;
            hideSequence = null;
        });
    }
}
