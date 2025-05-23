using UnityEngine;
using UnityEngine.Video;
public class UI_QTE : BaseUI
{
    private VideoPlayer player;
    private Event e;
    private void Start()
    {
        player = GetComponent<VideoPlayer>();
        e = FindObjectOfType<Event>();
        LoadData();
    }

    private void LoadData()
    {
        var data = DataManager.Instance.eventDict[e.id].background;
        player.clip = ResourceManager.Instance.LoadVideoClip(data);
    }
}
