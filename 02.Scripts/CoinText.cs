using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    public Animator animator;
    public CoinTextPool myPool;

    public TextMeshProUGUI text;

    float playTime;

    public void Play(float point)
    {
        text.text = $"+ {System.Math.Round(point, 2)}";
        animator.SetTrigger("Play");

        playTime = Time.time;
    }

    private void Update()
    {
        float currTime = Time.time;

        if (currTime - playTime >= 1f)
        {
            myPool.Destroy(this);
        }
    }
}
