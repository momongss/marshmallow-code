using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTextPool : PoolManager<CoinText>
{
    public static CoinTextPool I { get; private set; }

    void Awake()
    {
        I = this;
    }

    protected override CoinText __Instantiate(CoinText obj, Vector3 pos, Quaternion rot)
    {
        CoinText _coin = base.__Instantiate(obj, pos, rot);
        _coin.myPool = this;

        return _coin;
    }
}
