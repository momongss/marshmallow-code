using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImageReloadIndicator : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float animationRate = 0.4f;

    bool isShowing = false;

    private void Start()
    {
        Hide();
    }

    IEnumerator TextAnimation()
    {
        while (isShowing)
        {
            text.text = "장전중";
            yield return new WaitForSeconds(animationRate);

            text.text = "장전중.";
            yield return new WaitForSeconds(animationRate);

            text.text = "장전중..";
            yield return new WaitForSeconds(animationRate);
        }
    }

    public void Show()
    {
        isShowing = true;
        text.enabled = true;
        StartCoroutine(TextAnimation());
    }

    public void Hide()
    {
        isShowing = false;
        text.enabled = false;
    }
}
