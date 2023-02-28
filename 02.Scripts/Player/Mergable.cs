using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mergable : MonoBehaviour
{
    public enum State
    {
        Idle = 0,
        Indicated = 1,
        ReadyToMerge = 2
    }

    Player player;
    Player mergeMate;
    bool isDragged = false;

    [SerializeField] Collider coll;
    [SerializeField] MeshRenderer mergeIndicator;

    Material Mat_mergeIndicator;

    Color originColor = new Color(1, 0.9910396f, 0.6735849f);
    Color darkerColor = new Color(1, 0.5365348f, 0.3716981f);

    Rigidbody rigid;

    Transform model;

    private void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();

        model = transform.GetChild(0).GetChild(0);

        Mat_mergeIndicator = mergeIndicator.GetComponent<MeshRenderer>().material;
        Mat_mergeIndicator.SetColor("_Color", originColor);

        ChangeState(State.Idle);
    }

    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Idle:
                mergeIndicator.enabled = false;
                break;
            case State.Indicated:
                Mat_mergeIndicator.SetColor("_Color", originColor);
                mergeIndicator.enabled = true;
                break;
            case State.ReadyToMerge:
                Mat_mergeIndicator.SetColor("_Color", darkerColor);
                mergeIndicator.enabled = true;
                break;
            default:
                Debug.LogError($"처리안된 state {state}");
                break;
        }
    }

    private void OnMouseDown()
    {
        if (enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            isDragged = true;

            transform
                .DOScale(new Vector3(1.1f, 0.85f, 1.1f), 0.1f)
                .SetEase(Ease.Linear);

            model
                .DOMoveY(0.8f, 0.2f);

            player.ChangeState(Player.State.Grabbed);

            SoundManager.I.Grab02.Play();

            if (!isMaxLevel())
            {
                PlayerSquad.I.IndicateMergables(player.type, player.level);

                if (mergeMate)
                {
                    mergeMate.mergable.ChangeState(State.ReadyToMerge);
                    ChangeState(State.ReadyToMerge);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (enabled)
        {
            transform
                .DOScale(new Vector3(1f, 1f, 1f), 0.2f)
                .SetEase(Ease.Linear);

            model
                .DOMoveY(0.193f, 0.2f);

            if (!isMaxLevel())
            {
                PlayerSquad.I.DeIndicateMergables();
                if (mergeMate)
                {
                    PlayerSquad.I.MergePlayer(player, mergeMate);
                }
            }

            player.ChangeState(Player.State.Rest);

            SoundManager.I.Grab01.Play();

            isDragged = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDragged) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, 1 << Layer.TERRAIN))
            {
                Vector3 pos = hit.point;

                rigid.MovePosition(new Vector3(pos.x, transform.position.y, pos.z));
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (isDragged)
        {
            if (isMaxLevel()) return;

            if (collider.gameObject.layer == Layer.PLAYER)
            {
                Player p = collider.gameObject.GetComponent<Player>();
                if (p.type == player.type && p.level == player.level)
                {
                    // 이전 mergeMate 는 해제
                    if (mergeMate)
                    {
                        mergeMate.mergable.ChangeState(State.Indicated);
                    }

                    // 새로운 mergeMate 표시
                    mergeMate = p;
                    mergeMate.mergable.ChangeState(State.ReadyToMerge);
                    ChangeState(State.ReadyToMerge);
                }
            }
        } else
        {
            if (isMaxLevel()) return;

            if (collider.gameObject.layer == Layer.PLAYER)
            {
                Player p = collider.gameObject.GetComponent<Player>();
                if (p.type == player.type && p.level == player.level)
                {
                    mergeMate = p;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (isDragged)
        {
            if (isMaxLevel()) return;

            if (collider.gameObject.layer == Layer.PLAYER)
            {
                Player p = collider.gameObject.GetComponent<Player>();
                if (p.type == player.type && p.level == player.level && mergeMate == p)
                {
                    if (mergeMate)
                    {
                        mergeMate.mergable.ChangeState(State.Indicated);
                    }
                    ChangeState(State.Indicated);

                    mergeMate = null;
                }
            }
        } else
        {
            if (isMaxLevel()) return;

            if (collider.gameObject.layer == Layer.PLAYER)
            {
                Player p = collider.gameObject.GetComponent<Player>();
                if (mergeMate == p)
                {
                    mergeMate = null;
                }
            }
        }
    }

    bool isMaxLevel()
    {
        int maxLevel;
        switch (player.type)
        {
            case Player.Type.Magic:
                maxLevel = PlayerSquad.I.playerPrefabs_Magic.Length - 1;
                break;
            case Player.Type.Pistol:
                maxLevel = PlayerSquad.I.playerPrefabs_Pistol.Length - 1;
                break;
            case Player.Type.Bazooka:
                maxLevel = PlayerSquad.I.playerPrefabs_Pistol.Length - 1;
                break;

            default:
                Debug.LogError($"처리하셈 {player.type}");
                return false;
        }

        if (player.level > maxLevel)
        {
            Debug.LogError($"현재 레벨 : {player.level}, 최대레벨 : {maxLevel}");
        }

        return player.level == maxLevel;
    }
}
