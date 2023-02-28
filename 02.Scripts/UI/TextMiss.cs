using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMiss : MonoBehaviour
{
    public static TextMiss I { get; private set; }

    [SerializeField] Canvas Text_miss;

    private void Awake()
    {
        I = this;
    }

    public void Play(Vector3 pos)
    {
        Component text = PoolGeneral.I._Instantiate(Text_miss, pos, Quaternion.identity);
        PoolGeneral.I._Destroy(text, 1f);
    }
}
