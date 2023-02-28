using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsSetter : MonoBehaviour
{
    public int maxFrameRate = 60;
    void Start()
    {
        Application.targetFrameRate = maxFrameRate;
    }
}
