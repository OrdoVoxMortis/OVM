using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public static string NextSceneName {  get; private set; }

    public static void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }

}
