using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingleTon<LoadSceneManager>
{
    float time = 0;
    public string loadingScene; // 로딩 후 이동할 씬
    public AsyncOperation asyncOperation;
    protected override void Awake()
    {
        base.Awake();
    }
    public void LoadSceneWithLoading(string sceneName)
    {
        loadingScene = sceneName;
        StartCoroutine(LoadingAsync("Loading"));
    }

    IEnumerator LoadingAsync(string name)
    {
        asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false; //로딩이 완료되는대로 씬을 활성화할것인지

        while (!asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = true;
            yield return null;
        }
    }

    public void StartLoad()
    {
        asyncOperation.allowSceneActivation = true;
    } 
}