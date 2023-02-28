using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BillboardOnce : MonoBehaviour
{
    Transform target;

    private void Awake()
    {
        target = Camera.main.transform;
        transform.LookAt(target);
    }
}
