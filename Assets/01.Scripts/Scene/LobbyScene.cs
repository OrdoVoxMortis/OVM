using TMPro;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");
        //UIManager.Instance.ShowUI<UI_Start>("Start_UI");
        //UIManager.Instance.ShowUI<UI_SaveLoad>("UI_SaveLoad");
        //UIManager.Instance.ShowUI<UI_Volume>("UI_Volume");

    }
}
