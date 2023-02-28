using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader I { get; private set; }

    [SerializeField]
    Image progressBar;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(I);
        }

        I = this;
        DontDestroyOnLoad(gameObject);
        print(I);
    }

    public string nextSceneName;

    public void LoadSceneWithLoading(string sceneName)
    {
        nextSceneName = sceneName;

        SceneManager.LoadScene(SceneName.LoadingScene);
    }

    public void LoadNextSceneAsync()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        float timer = 0.0f;

        if (progressBar != null)
        {
            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;
                if (op.progress < 0.9f) { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); if (progressBar.fillAmount >= op.progress) { timer = 0f; } }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                    if (progressBar.fillAmount == 1.0f) { op.allowSceneActivation = true; yield break; }
                }
            }
        }
    }
}
