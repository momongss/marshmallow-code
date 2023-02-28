using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImages : MonoBehaviour
{
    public static PlayerImages I { get; private set; }

    public Sprite[] sprites_Pistol;
    public Sprite[] sprites_Bazooka;

    private void Awake()
    {
        I = this;
    }
}
