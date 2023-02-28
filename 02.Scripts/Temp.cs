using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

// [ExecuteInEditMode]
public class Temp : MonoBehaviour
{
    public GameObject button;
    public GameObject[] playerList;

    //private void OnValidate()
    //{
    //    foreach (var g in playerList)
    //    {
    //        int level = Utils.ToInt(g.name.Split(" ")[1]);

    //        GameObject _b = Instantiate(button, transform);
    //        _b.GetComponent<UICollectionItem>().level = level;

    //        Instantiate(g, _b.transform);
    //    }
    //}

    //private void OnValidate()
    //{
    //    foreach (var t in Utils.GetChildren(transform))
    //    {
    //        var t2 = t.GetChild(0);
    //        t2.localPosition = new Vector3(0f, -0.02f, 0f);
    //        t2.localRotation = Quaternion.Euler(new Vector3(15.455f, 6.303f, -14.52f));
    //        t2.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //    }
    //}
}
