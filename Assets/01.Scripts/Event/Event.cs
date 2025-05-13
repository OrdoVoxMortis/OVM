using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Event : TimelineElement
{
    //QTEManager
    public string ImageName { get; private set; }
    public string Description { get; private set; }
    public string BgmName { get; private set; }

    private PostProcessingToggle postProcessingToggle; // 추후 수정
    private QTEManager qteManager;
    public bool IsCollect { get; set; } // 해금
    private void Awake()
    {
        postProcessingToggle = FindObjectOfType<PostProcessingToggle>(); // 추후수정
        qteManager = GetComponent<QTEManager>();
    }
    private void Start()
    {
        LoadData();
        qteManager.eventBgm = BgmName;
    }
    protected virtual void LoadData()
    {
        var data = DataManager.Instance.eventDict[id];
        Name = data.name;
        Description = data.description;
        if (DataManager.Instance.musicDict.TryGetValue(data.bgmId, out var bgm))
        {
            BgmName = bgm.name;
        }
    }
    public override string GetInteractComponent()
    {
        if (!IsActive) return "E키를 눌러 활성화";
        else return "X키를 눌러 비활성화";
    }

    public override void OnInteract()
    {
        if (!IsActive)
        {
            FindObjectOfType<PostProcessingToggle>().EnablePostProcessing();
            TimelineManager.Instance.AddBlock(this);
            //RhythmManager.Instance.rhythmActions.Add(qteManager);
            IsActive = true;
            IsCollect = true;
            Debug.Log("이벤트 데이터 추가!");
        }
        else
        {
            //RhythmManager.Instance.rhythmActions.Remove(qteManager);
            TimelineManager.Instance.DestroyBlock(this);
            IsActive = false;
            IsCollect = false;
            Debug.Log("이벤트 데이터 삭제!");
        }
       
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }

    public override void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }
}

   
