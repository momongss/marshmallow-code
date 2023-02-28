using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;

public class FirebaseStorageManager : MonoBehaviour
{
    public bool isLoadResources = true;

    private void Awake()
    {
        Utils.CreateDirectory(Application.persistentDataPath + "/Stage");
        Utils.CreateDirectory(Application.persistentDataPath + "/Stats");

        #if UNITY_ANDROID
        if (isLoadResources)
        {
            DownLoadResources();
        }
        #endif
    }

    public void DownLoadResources()
    {
        print(Application.persistentDataPath);

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://marshmallowproject-9a6aa.appspot.com/");

        DownLoadFile(storage_ref, "stage-info.json", "/Stage/stage-info.json");
        DownLoadFile(storage_ref, "playerStats.csv", "/Stats/playerStats.csv");
        DownLoadFile(storage_ref, "enemyStats.csv", "/Stats/enemyStats.csv");
    }

    void DownLoadFile(StorageReference storage_ref, string originPath, string localPath)
    {
        StorageReference isstorage_ref = storage_ref.Child(originPath);

        string local_url = Application.persistentDataPath + localPath;

        isstorage_ref.GetFileAsync(local_url).ContinueWith(file_task =>
        {
            if (!file_task.IsFaulted && !file_task.IsCanceled)
            {
                Debug.Log($"downloadfirebase Succcess {localPath}");
            }
            else
            {
                Debug.Log($"downloadfirebase fail {localPath}");
                Debug.LogError(file_task.Exception.ToString());
            }
        });
    }
}