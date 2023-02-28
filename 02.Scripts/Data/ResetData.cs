using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetData : MonoBehaviour
{
    public void resetData()
    {
        JsonData.RemoveAllData();
    }
}
