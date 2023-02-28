using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource[] bgmList;

    private void Awake()
    {
        SelectBGMRandom();
    }

    void SelectBGMRandom()
    {
        int selected = Random.Range(0, bgmList.Length);

        for (int i = 0; i < bgmList.Length; ++i)
        {
            if (i == selected)
            {
                bgmList[i].enabled = true;
            } else
            {
                bgmList[i].enabled = false;
            }
        }
    }
}
