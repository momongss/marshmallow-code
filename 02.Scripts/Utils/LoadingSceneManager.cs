using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public float defaultDelay = 3.5f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(defaultDelay);

        SceneManager.LoadScene(SceneName.PlayScene);
    }
}
