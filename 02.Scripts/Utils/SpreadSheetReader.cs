using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpreadSheetReader : MonoBehaviour
{
    private void Start()
    {
        string url = "https://docs.google.com/spreadsheets/d/1BaVXymdY6KsFV5skSVxndWq_8Bxw-bFR-Peb5D26aSI/export?format=csv";

        StartCoroutine(SpreadSheetReader.Download(url));
    }

    public static IEnumerator Download(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        Set(www.downloadHandler.text);
    }

    static void Set(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                print(column[j]);
            }
        }
    }
}
