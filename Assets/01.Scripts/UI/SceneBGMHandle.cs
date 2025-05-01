using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBGMHandle : MonoBehaviour
{
    [SerializeField] private string bgmName;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgmName);
    }
}
