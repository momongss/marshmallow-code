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
                Debug.LogError($"처리하셈 {type}");
                return;
        }

        Say(talkList[Random.Range(0, talkList.Length)], timeout);
    }

    public void Say(string[] talkList, float timeout = 5f)
    {
        Say(talkList[Random.Range(0, talkList.Length)], timeout);
    }

    string[] talk_self = new string[] { "...", "아함", "졸리다", "심심해라" };
    string[] talk_grabbed = new string[] { "안녕!", "아이고 깜짝이야", "반가워!", "이거놔", "블라블라" };
    string[] talk_fever = new string[] { "공격!", "으아아" };
    public static string[] talk_battleStart = new string[] {
        "이길 수 있나?",
        "무서워..",
        "이기자!",
        "두두두두",
        "잼있겟다",
    };

    public static string[] talk_battle = new string[]
    {
        "으아아 오지마",
        "드루와",
        "크아아",
        "날려버려!",
        "공격해!"
    };

    public static string[] talk_HPLow = new string[]
    {
        "아이고",
        "지고 말거야",
        "으아아아!",
        "안되!"
    };

    public static string[] talk_battleWin= new string[]
    {
        "휴우..",
        "이겼다!",
        "역시"
    };

    public string[] talk_happy = new string[] { 
        "고마워!",
        "힘이난다",
        "좋아!"
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
