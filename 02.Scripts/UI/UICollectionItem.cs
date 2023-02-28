using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionItem : MonoBehaviour
{
    public Image image_marsh;

    public Color activatedColor;
    public Color inactivatedColor;

    public Player.Type type;
    public int level;

    Button button;

    bool isActivated = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (isActivated)
            {
                UICharacterInfo.I.ShowInfo(type, level);
            }
        });
    }

    public void Activate()
    {
        isActivated = true;
        image_marsh.color = activatedColor;
    }

    public void Inactivate()
    {
        isActivated = false;
        image_marsh.color = inactivatedColor;
    }
}
