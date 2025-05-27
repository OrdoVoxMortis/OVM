using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(LoadNextScene());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator LoadNextScene()
    {
        Debug.Log("코루틴 시작");
        string targetScene = LoadSceneManager.Instance.loadingScene;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(targetScene);
        asyncOp.allowSceneActivation = false;

        float waitTime = 5f; // 로딩씬을 보여줄 시간 (필요 시 조정)
        float timer = 0f;

        while (!asyncOp.isDone)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                Debug.Log($"progress = {asyncOp.progress}");
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Loading" && scene.name != "Lobby_Scene" && (SaveManager.Instance.isReplay || SaveManager.Instance.eventReplay))
        {
            Debug.Log("씬 전환 후 초기화 시작");

            StartCoroutine(SaveManager.Instance.DelayInit());
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}



