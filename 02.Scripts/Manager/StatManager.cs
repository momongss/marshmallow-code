using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    
}
