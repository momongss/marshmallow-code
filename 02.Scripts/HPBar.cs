using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI Text_hp;

    public Canvas canvas;

    public float backDelay = 0.5f;
    public float backSpeed = 1f;

    public bool isOnlyShowOnSet = true;

    private void OnValidate()
    {
        canvas = GetComponent<Canvas>();
    }

    public void SetMaxHP(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;

        Text_hp.SetText(hp.ToString());

        if (isOnlyShowOnSet)
        {
            canvas.enabled = false;
        }
    }

    public void SetHP(float hp)
    {
        slider.value = hp;
        Text_hp.SetText(hp.ToString());
    }

    Coroutine hideRoutine = null;

    public void SetHP(float hp, float showTime)
    {
        canvas.enabled = true;
        SetHP(hp);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(Hide(showTime));
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.enabled = false;

        hideRoutine = null;
    }
}
