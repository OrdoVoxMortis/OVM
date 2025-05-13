using TMPro;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public MissionNote[] notes; 
    protected override void Init()
    {
        base.Init();
        UI_Quest uI_Quest = UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");

        for(int i = 0; i < notes.Length; i++)
        {
            notes[i].questUI = uI_Quest;
        }
        //UIManager.Instance.ShowUI<UI_Start>("Start_UI");
        //UIManager.Instance.ShowUI<UI_SaveLoad>("UI_SaveLoad");
        //UIManager.Instance.ShowUI<UI_Volume>("UI_Volume");

    }
}
