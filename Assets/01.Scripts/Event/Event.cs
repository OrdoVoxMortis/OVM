using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using UnityEngine;

public class Event : TimelineElement
{
    //QTEManager
    public string ImageName { get; private set; }
    public string Description { get; private set; }
    public string BgmName { get; private set; }

    private PostProcessingToggle postProcessingToggle; // 추후 수정
    private QTEManager qteManager;
    private MeshRenderer meshRenderer;
    private Material ghostOutline;
    public bool IsCollect { get; set; } // 해금

    private void Awake()
    {
        postProcessingToggle = FindObjectOfType<PostProcessingToggle>(); // 추후수정
        qteManager = GetComponent<QTEManager>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        LoadData();
        qteManager.eventBgm = BgmName;
        ghostOutline = ResourceManager.Instance.LoadMaterial("OutlineGhost");
    }
    protected virtual void LoadData()
    {
        var data = DataManager.Instance.eventDict[id];
        Name = data.name;
        Description = data.description;
        BgmName = data.bgmName;
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
            AddOutlineMaterial();
            Debug.Log("이벤트 데이터 추가!");

            var sendBlockEvent = new CustomEvent("block_clicked")
            {
                ["block_id"] = "E" + id
            };
            AnalyticsService.Instance.RecordEvent(sendBlockEvent);
        }
        else
        {
            //RhythmManager.Instance.rhythmActions.Remove(qteManager);
            TimelineManager.Instance.DestroyBlock(this);
            IsActive = false;
            IsCollect = false;
            RemoveOutlineMaterial();
            Debug.Log("이벤트 데이터 삭제!");
        }
        TimelineManager.Instance.OnBlockUpdate?.Invoke();
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }

    public override void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }

    private void AddOutlineMaterial()
    {
        if (meshRenderer != null)
        {
            var curMaterials = meshRenderer.materials.ToList();
            bool exists = curMaterials.Any(m => m.name.StartsWith(ghostOutline.name));
            if (!exists)
            {
                curMaterials.Add(ghostOutline);
                meshRenderer.materials = curMaterials.ToArray();
            }
        }
    }

    private void RemoveOutlineMaterial()
    {
        if (meshRenderer != null)
        {
            var curMaterials = meshRenderer.materials.ToList();
            curMaterials.RemoveAll(m => m.name.StartsWith(ghostOutline.name));
            meshRenderer.materials = curMaterials.ToArray();
        }
    }

    public void ToggleOutline()
    {
        if (!GameManager.Instance.SimulationMode) RemoveOutlineMaterial();
        else AddOutlineMaterial();
    }
}

   
