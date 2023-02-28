using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMesh textMesh;

    public void Init(string text)
    {
        textMesh.text = text;

        PoolGeneral.I._Destroy(this, 0.7f);
    }
}
