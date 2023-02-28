using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform target;

    private void Awake()
    {
        target = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(target);
    }
}
