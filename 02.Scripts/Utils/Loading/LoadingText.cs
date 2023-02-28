using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float animSpeed = 0.5f;

    private void Reset()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    IEnumerator Start()
    {
        for (int i = 0; i < 200; i++)
        {
            text.SetText("로딩중");
            yield return new WaitForSeconds(animSpeed);
            text.SetText("로딩중.");
            yield return new WaitForSeconds(animSpeed);
            text.SetText("로딩중..");
            yield return new WaitForSeconds(animSpeed);
            text.SetText("로딩중...");
            yield return new WaitForSeconds(animSpeed);
        }
    }
}
