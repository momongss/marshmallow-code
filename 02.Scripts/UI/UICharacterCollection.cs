using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCollection : MonoBehaviour
{
    public static UICharacterCollection I { get; private set; }

    public GameObject collectionGunner;

    public Sprite[] sprites_Pistol;
    public Sprite[] sprites_Bazooka;

    Transform transform_gunnerList;

    public UICollectionItem prefab_item;

    private void Awake()
    {
        I = this;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        transform_gunnerList = collectionGunner.transform.GetChild(0).GetChild(0);
    }

    public void ShowBazookaCollection()
    {
        int maxLevel = PlayerSquad.I.GetCurrMaxLevel(Player.Type.Bazooka);

        foreach (Transform t in Utils.GetChildren(transform_gunnerList))
        {
            Destroy(t.gameObject);
        }

        foreach (Player p in PlayerSquad.I.playerPrefabs_Bazooka)
        {
            Sprite sprite_player = sprites_Bazooka[p.level];
            UICollectionItem item = Instantiate(prefab_item, transform_gunnerList);

            item.type = p.type;
            item.level = p.level;

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.image_marsh.sprite = sprite_player;

            if (p.level <= maxLevel)
            {
                item.Activate();
            }
            else
            {
                item.Inactivate();
            }
        }
    }

    public void ShowGunnerCollection()
    {
        int maxLevel = PlayerSquad.I.GetCurrMaxLevel(Player.Type.Pistol);

        foreach (Transform t in Utils.GetChildren(transform_gunnerList))
        {
            Destroy(t.gameObject);
        }

        foreach (Player p in PlayerSquad.I.playerPrefabs_Pistol)
        {
            Sprite sprite_player = sprites_Pistol[p.level];
            UICollectionItem item = Instantiate(prefab_item, transform_gunnerList);

            item.type = p.type;
            item.level = p.level;

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.image_marsh.sprite = sprite_player;

            if (p.level <= maxLevel)
            {
                item.Activate();
            } else
            {
                item.Inactivate();
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        ShowGunnerCollection();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
