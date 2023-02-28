using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour
{
    public float speedUpRatio = 1.8f;
    public float speedUpDuration = 7f;

    public float powerUpRatio = 3f;
    public float powerUpDuration = 5f;

    public float hp;

    public SpawnItem[] itemList;

    public float spawnRate = 10f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Spawn();
        }
    }

    void Spawn()
    {
        SpawnItem _item = (SpawnItem)PoolGeneral.I._Instantiate(
            itemList[Random.Range(0, itemList.Length)], 
            Utils.GetRandomPos(PlayerSquad.I.transform.position, 3f, 4f)
            ,Quaternion.identity
            );

        switch (_item.type)
        {
            case SpawnItem.Type.HP:
                _item.Spawn(hp, 0);
                break;
            case SpawnItem.Type.Speed:
                _item.Spawn(speedUpRatio, speedUpDuration);
                break;
            case SpawnItem.Type.Power:
                _item.Spawn(powerUpRatio, powerUpDuration);
                break;

            default:
                Debug.LogError($"Ã³¸®ÇÏ¼À {_item.type}");
                break;
        }
    }
}
