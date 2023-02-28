using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappyManager : MonoBehaviour
{
    public static HappyManager I { get; private set; }

    int maxHappiess = 100;
    public int happiness;

    const string path = "/happiess.json";

    public float happyPower;

    [SerializeField] TextMeshProUGUI text_happy;
    [SerializeField] TextMeshProUGUI text_happyPower;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        LoadData();
        happyPower = GetHappyPower();
        text_happy.SetText($"{happiness}");
        text_happyPower.SetText($"(x{happyPower})");

        LayoutRebuilder.ForceRebuildLayoutImmediate(text_happyPower.transform.parent.GetComponent<RectTransform>());
    }

    public void AddHappiess()
    {
        addHappiess(1);

        for (int i = 0; i < PlayerSquad.I.playerList.Count; i++)
        {
            Player p = PlayerSquad.I.playerList[i];
            p.PowerUp();
        }
    }

    Coroutine saveRoutine = null;

    void addHappiess(int amount)
    {
        happiness += amount;
        if (happiness > maxHappiess)
        {
            happiness = maxHappiess;
            return;
        }

        text_happy.SetText($"{happiness}");

        if (saveRoutine != null)
        {
            StopCoroutine(saveRoutine);
        }

        saveRoutine = StartCoroutine(SaveData());

        happyPower = GetHappyPower();
        text_happyPower.SetText($"(x{happyPower})");

        LayoutRebuilder.ForceRebuildLayoutImmediate(text_happyPower.transform.parent.GetComponent<RectTransform>());
    }

    public float GetHappyPower()
    {
        if (happiness < 20)
        {
            return 1f;
        } else if (happiness < 40)
        {
            return 1.2f;
        } else if (happiness < 60)
        {
            return 1.4f;
        } else if (happiness < 80)
        {
            return 1.6f;
        } else if (happiness < 100)
        {
            return 1.8f;
        } else
        {
            return 2f;
        }
    }

    IEnumerator SaveData()
    {
        yield return new WaitForSeconds(2f);
        JsonData.SaveJson($"{happiness}", path);

        saveRoutine = null;
    }

    void LoadData()
    {
        if (!JsonData.isFileExist(path))
        {
            happiness = 0;
            JsonData.SaveJson($"{happiness}", path);
        }
        else
        {
            happiness = Utils.ToInt(JsonData.LoadJson(path));
        }
    }
}
