using UnityEngine;

public class SceneBGMHandle : MonoBehaviour
{
    [SerializeField] private string bgmName;

    private void Start()
    {
        bgmName = "Background";
    }
}
