using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyScene : BaseScene
{
    [SerializeField] private Canvas canvas0; // sortingOrder = 0 Canvas (Hierarchy에서 드래그 할당)
    public MissionNote[] notes;
    public void StartSaturation(IEnumerator cor)
    {
        StartCoroutine(cor);
    }
    protected override void Init()
    {
        base.Init();
        //UI_Quest uI_Quest = UIManager.Instance.ShowUI<UI_Quest>("UI_Quest");
        //uI_Quest.transform.SetParent(canvas0.transform, false);
        //for (int i = 0; i < notes.Length; i++)
        //{
        //    notes[i].questUI = uI_Quest;
        //}
        //UIManager.Instance.ShowUI<UI_Start>("Start_UI");
        //UIManager.Instance.ShowUI<UI_SaveLoad>("UI_SaveLoad");
        //UIManager.Instance.ShowUI<UI_Volume>("UI_Volume");

    }
}
