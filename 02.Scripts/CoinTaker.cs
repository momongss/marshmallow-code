using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTaker : MonoBehaviour
{
    public static CoinTaker I { get; private set; }

    private void Awake()
    {
        I = this;
    }
}
