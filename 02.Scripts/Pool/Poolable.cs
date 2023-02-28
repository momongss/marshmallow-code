using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable<T> : MonoBehaviour where T : MonoBehaviour
{
    public PoolManager<T> myPool;
}
