using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : MonoBehaviour
{
    Image image;

    public bool isShowing = true;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
    }

    public virtual void Show()
    {
        isShowing = true;
        image.enabled = true;
    }

    public virtual void Hide()
    {
        isShowing = false;
        image.enabled = false;
    }
}
