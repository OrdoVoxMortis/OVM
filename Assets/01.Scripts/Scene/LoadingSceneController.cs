using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        string targetScene = LoadSceneManager.Instance.loadingScene;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(targetScene);
        asyncOp.allowSceneActivation = false;

        float waitTime = 5f; // 로딩씬을 보여줄 시간 (필요 시 조정)
        float timer = 0f;

        while (!asyncOp.isDone)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime && asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
