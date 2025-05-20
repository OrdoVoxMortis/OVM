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
        { //isDone는 로딩이 완료되었는지 확인하는 변수
          //time += Time.deltaTime; //시간을 더해줌
          //print(asyncOperation.progress); //로딩이 얼마나 완료되었는지 0~1의 값으로 보여줌

            //이건 로딩이 너무 빨라서 작성한거라, 무거운 씬 로딩할땐 시간 체크하는 부분은
            //생략해도 무방하다!
            //if (time > 5)
            //{ //3초 기다림(변동가능)
            //씬 활성화
            asyncOperation.allowSceneActivation = true;
            //}

            yield return null;
        }
    }

    public void StartLoad()
    {
        asyncOperation.allowSceneActivation = true;
    } 
}