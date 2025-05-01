using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneBGMHandle : MonoBehaviour
{
    [SerializeField] private string bgmName;

    private void Start()
    {
        bgmName = "Background";
    }
}
