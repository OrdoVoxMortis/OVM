using TMPro;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");


    }
}
