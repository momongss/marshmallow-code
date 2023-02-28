using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public enum EType
    {
        BattleStart = 0,
        Fever = 1,
        Grabbed = 2,
        SelfTalk = 3,
    }

    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        GetComponent<Canvas>().enabled = true;
        text.enabled = false;
    }

    Coroutine routine;

    public void Say(string _text, float timeout = 5f)
    {
        if (routine != null)
        {
            return;
        }

        routine = StartCoroutine(Routine(_text, timeout));
    }

    public void Say(EType type, float timeout = 5f)
    {
        string[] talkList;

        switch (type)
        {
            case EType.BattleStart:
                talkList = talk_battleStart;
                break;
            case EType.Fever:
                talkList = talk_fever;
                break;
            case EType.Grabbed:
                talkList = talk_grabbed;
                break;
            case EType.SelfTalk:
                talkList = talk_self;
                break;
            default:
                Debug.LogError($"ó���ϼ� {type}");
                return;
        }

        Say(talkList[Random.Range(0, talkList.Length)], timeout);
    }

    public void Say(string[] talkList, float timeout = 5f)
    {
        Say(talkList[Random.Range(0, talkList.Length)], timeout);
    }

    string[] talk_self = new string[] { "...", "����", "������", "�ɽ��ض�" };
    string[] talk_grabbed = new string[] { "�ȳ�!", "���̰� ��¦�̾�", "�ݰ���!", "�̰ų�", "�����" };
    string[] talk_fever = new string[] { "����!", "���ƾ�" };
    public static string[] talk_battleStart = new string[] {
        "�̱� �� �ֳ�?",
        "������..",
        "�̱���!",
        "�εεε�",
        "���ְٴ�",
    };

    public static string[] talk_battle = new string[]
    {
        "���ƾ� ������",
        "����",
        "ũ�ƾ�",
        "��������!",
        "������!"
    };

    public static string[] talk_HPLow = new string[]
    {
        "���̰�",
        "���� ���ž�",
        "���ƾƾ�!",
        "�ȵ�!"
    };

    public static string[] talk_battleWin= new string[]
    {
        "�޿�..",
        "�̰��!",
        "����"
    };

    public string[] talk_happy = new string[] { 
        "����!",
        "���̳���",
        "����!"
    };

    public void SpeakSelf()
    {
        Say(talk_self[Random.Range(0, talk_self.Length)], 3f);
    }

    public void Grabbed()
    {
        Say(talk_grabbed[Random.Range(0, talk_grabbed.Length)], 3f);
    }

    IEnumerator Routine(string _text, float timeout)
    {
        text.SetText(_text);
        text.enabled = true;

        yield return new WaitForSeconds(timeout);

        text.enabled = false;

        routine = null;
    }
}
