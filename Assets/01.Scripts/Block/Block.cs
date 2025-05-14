
using GoogleSheet.Core.Type;
using UnityEngine;
using Hamster.ZG.Type;
using System.Collections.Generic;
using System.Linq;

[UGS(typeof(BlockType))]
public enum BlockType
{
    None,
    Tool, // 도구
    Food, // 음식
    Terrain // 지형
}
[UGS(typeof(BlockAction))]
public enum BlockAction
{
    None,
    Collect, // 수집
    Use, // 사용
    Move // 이동
}

[UGS(typeof(CombineType))]
public enum CombineType
{
    None, // 규칙 없음
    AllowByType, // 특정 유형 허용
    AllowSpecific // 특정 블럭 허용
}

public class Block : TimelineElement
{
    public BlockType Type { get; private set; } // 유형
    public BlockAction Action { get; private set; } // 행동

    public float FixedTime {  get; private set; } // 필수 시간
    public float AfterFlexibleMarginTime {  get; private set; } // 최대 뒤 유동 시간
    public float CurrentAfterFlexTime { get; private set; } // 현재 뒤 유동 시간

    public CombineRule NextCombineRule { get; private set; } // 후속 조합 규칙
    public CombineRule PreCombineRule { get; private set; } // 선행 조합 규칙

    public Block NextBlock { get; set; } // 다음 블럭
    public Block PrevBlock { get; set; } // 이전 블럭

    public bool IsDeathTrigger {  get; private set; } // 사망 트리거

    public AnimationClip SuccessSequence {  get; private set; } // 성공 노트 시퀀스
    public AnimationClip FailSequence {  get; private set; } // 실패 노트 시퀀스
    public AnimationClip FixedSequence {get; private set;} // 고정 시간 노트 시퀀스
    public AnimationClip AfterFlexSequence {get; private set;} // 뒤 유동 시간 노트 시퀀스
    public AudioClip BlockSound { get; private set; } // 블럭 사운드

    protected GhostManager ghostManager;
    public bool IsSuccess { get; set; } // 조합 성공인지
    protected PostProcessingToggle postProcessingToggle; // 추후 수정
    protected GameObject clone; // 클론 위치
    protected Animator animator;
    protected MeshRenderer blockMesh;
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected Material ghostOutline;

    protected virtual void Awake()
    {
        LoadData();
        ghostManager = GetComponent<GhostManager>();
        if (ghostManager == null) return;

        postProcessingToggle = FindObjectOfType<PostProcessingToggle>(); // 추후수정

        DataToGhost();

        animator = transform.GetChild(0).GetComponent<Animator>();
        clone = transform.GetChild(1).gameObject;
        if (animator.transform.childCount > 0)
        {
            skinnedMeshRenderer = transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
        }
        else blockMesh = transform.GetChild(2).GetComponent<MeshRenderer>();

        GameManager.Instance.OnSimulationMode += ToggleGhost;

        ghostOutline = ResourceManager.Instance.LoadMaterial("OutlineGhost");
    }

    public virtual void LoadData()
    {
        var data = DataManager.Instance.blockDict[id];

        Type = data.type;
        Action = data.action;
        Name = data.blockName;
        PreCombineRule = data.prevCombineRule;
        NextCombineRule = data.nextCombineRule;
        FixedTime = data.fixedTime;
        AfterFlexibleMarginTime = data.afterFlexibleMarginTime;
        IsDeathTrigger = data.isDeathTrigger;
        CurrentAfterFlexTime = AfterFlexibleMarginTime;

        SuccessSequence = ResourceManager.Instance.LoadAnimationClip(data.successSequence);
        FailSequence = ResourceManager.Instance.LoadAnimationClip(data.failSequence);
        

        FixedSequence = ResourceManager.Instance.LoadAnimationClip(data.fixedSequence);
        AfterFlexSequence = ResourceManager.Instance.LoadAnimationClip(data.afterFlexSequence);
        if (!string.IsNullOrEmpty(data.blockSound) && ResourceManager.Instance.SfxList.TryGetValue(data.blockSound, out var clip))
        {
            BlockSound = clip;
        }
    }

    public override void OnInteract()
    {
        if (!IsActive)
        {
            AddOutlineMaterial();
            FindObjectOfType<PostProcessingToggle>().EnablePostProcessing();
            TimelineManager.Instance.AddBlock(this);
        }
        else
        {
            RemoveOutlineMaterial();
            TimelineManager.Instance.DestroyBlock(this);
            ghostManager.RemoveGhost();
            Debug.Log("블럭 데이터 삭제!");
        }

        BlockManager.Instance.OnBlockUpdate?.Invoke();
      
    }

    public override string GetInteractComponent()
    {
        if (!IsActive) return "E키를 눌러 활성화";
        else return "X키를 눌러 비활성화";
    }

    public void DataToGhost()
    {
        if (ghostManager == null) return;
        ghostManager.playerTrans = transform.GetChild(1);
        ghostManager.ghostClip = FixedSequence;
        ghostManager.ghostPrefabs = transform.GetChild(0).gameObject;
        ghostManager.bpm = GameManager.Instance.Bpm;
        ghostManager.blockSound = DataManager.Instance.blockDict[id].blockSound;
    }

    public virtual void SetGhost()
    {
        if(ghostManager == null) return;    
        var animatorController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        if (IsSuccess)
        {
            ghostManager.ghostClip = SuccessSequence;
    
        }
        else
        {
            ghostManager.ghostClip = FailSequence;

        }
        foreach (var clip in animatorController.animationClips)
        {
            animatorController[clip.name] = ghostManager.ghostClip;
        }
        animator.runtimeAnimatorController = animatorController;
        ghostManager.SetBeatList(ghostManager.beats, ghostManager.pointNoteList, ghostManager.bpm);
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }

    private void ToggleGhost()
    {
        foreach (Transform child in clone.transform)
        {
            Debug.Log(GameManager.Instance.SimulationMode);
            child.gameObject.SetActive(!GameManager.Instance.SimulationMode);
            if (GameManager.Instance.SimulationMode) RemoveOutlineMaterial();
            else AddOutlineMaterial();

        }
    }

    public override void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }

    private void AddOutlineMaterial()
    {
        if (blockMesh != null)
        {
            var curMaterials = blockMesh.materials.ToList();
            bool exists = curMaterials.Any(m => m.name.StartsWith(ghostOutline.name));
            if (!exists)
            {
                curMaterials.Add(ghostOutline);
                blockMesh.materials = curMaterials.ToArray();
            }
        }
        else
        {
            var curMaterials = skinnedMeshRenderer.materials.ToList();
            bool exists = curMaterials.Any(m => m.name.StartsWith(ghostOutline.name));
            if (!exists)
            {
                curMaterials.Add(ghostOutline);
                skinnedMeshRenderer.materials = curMaterials.ToArray();
            }
        }
    }

    private void RemoveOutlineMaterial()
    {
        if (blockMesh != null)
        {
            var curMaterials = blockMesh.materials.ToList();
            curMaterials.RemoveAll(m => m.name.StartsWith(ghostOutline.name));
            blockMesh.materials = curMaterials.ToArray();
        }
        else
        {
            var curMaterials = skinnedMeshRenderer.materials.ToList();
            curMaterials.RemoveAll(m => m.name.StartsWith(ghostOutline.name));
            skinnedMeshRenderer.materials = curMaterials.ToArray();
        }
    }
}
