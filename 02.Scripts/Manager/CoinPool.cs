using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : PoolManager<Coin>
{
    private void Start()
    {
        if (prefab)
        {
            prefab.myCoinPool = this;
        }
    }

    protected override Coin __Instantiate(Coin obj, Vector3 pos, Quaternion rot)
    {
        Coin _coin = base.__Instantiate(obj, pos, rot);
        _coin.myCoinPool = this;

        return _coin;
    }
}
