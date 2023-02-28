using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBackBar : MonoBehaviour
{
    public Slider slider;
    public float smoothing;
    float HP;

    public void SetMaxHP(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
        HP = hp;
    }

    public void SetHP(float hp)
    {
        StartCoroutine(_setHp(hp));
    }

    IEnumerator _setHp(float hp)
    {
        yield return new WaitForSeconds(0.1f);
        HP = hp;
    }

    private void Update()
    {
        if (slider.value >= HP)
        {
            slider.value -= smoothing * Time.deltaTime;
        }
    }
}
