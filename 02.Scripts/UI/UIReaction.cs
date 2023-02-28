using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIReaction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Animator anim;

    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        anim = text.GetComponent<Animator>();
    }

    public virtual void Say(string word, float sayTime = 3f)
    {
        text.text = word;
        text.enabled = true;

        anim.SetTrigger("Play");

        StartCoroutine(StopSay(sayTime));
    }

    public void Init()
    {
        gameObject.SetActive(true);
        text.enabled = false;
        StopAllCoroutines();
    }

    IEnumerator StopSay(float time)
    {
        yield return new WaitForSeconds(time);

        text.enabled = false;
    }
}
