using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_Stage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int start = 28;
        int end = 701;

        string s = "";

        for (int i = start; i < end; ++i)
        {
            s += ",{ \"stage\":" + $"{i}" + "\n\n}";
        }

        print(s);
    }
}
