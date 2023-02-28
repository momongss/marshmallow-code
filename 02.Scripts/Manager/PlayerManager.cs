using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player[] playerPistolPrefabs;
    [SerializeField] Player[] playerRiflePrefabs;
    [SerializeField] Player[] playerShotGunPrefabs;

    public void GetPlayerPrefab(Player.Type type, int level)
    {
        Player player;

        switch (type)
        {
            case Player.Type.Pistol:
                player = playerPistolPrefabs[level];
                break;
            case Player.Type.Rifle:
                player = playerRiflePrefabs[level];
                break;
            case Player.Type.ShotGun:
                player = playerShotGunPrefabs[level];
                break;

            default:
                Debug.Log($"처리되지 않은 player 타입 {type}");
                break;
        }


    }
}
