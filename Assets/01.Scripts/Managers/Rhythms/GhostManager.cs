using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour, IRhythmActions
{
    public Transform playerTrans; //고스트 시작 위치
    public Vector3 direction; //고스트가 생성되는 방향

    public Vector3 rotateAngle; //고스트가 생성된 후 회전각도

    public float ghostGaps;

    [HideInInspector]
    public float bpm;

    public List<float> beats;    //입력 받을 패턴 리스트
    public List<bool> pointNoteList; //true인 경우 point note

    public List<Ghost> ghosts;
    public List<float> checkTimes; //나중에 ghost로 이동 

    public GameObject ghostOriginal; //하이러키에 있는 오브젝트를 넣을 것
    public AnimationClip ghostClip;

    [HideInInspector]
    public float curTime;
    [HideInInspector]
    public bool isPlaying;
    private int curIndex = 0;

    [HideInInspector]
    public string[] hitSound = new string[2]; //0은 일반 노트 //1은 포인트 노트
    public string blockSound;

    private GameObject ghostCurTiming;

    private bool isReplay;
    private float tempTime;
    public Material ghostMat;
    public Material outlineMat;

    public bool isAnimMatch;
    private float animMatch;

    // Start is called before the first frame update
    void Start()
    {
        ghosts = new List<Ghost>();
        checkTimes = new List<float>();
        curIndex = 0;
        isPlaying = false;
        hitSound[0] = "Note_N1";
        hitSound[1] = "Note_P1";
        isReplay = SaveManager.Instance.isReplay;

        //RhythmManager.Instance.rhythmActions.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;
       
        if (curIndex >= ghosts.Count)
        {
            isPlaying = false;
            ghostOriginal.GetComponent<GhostAnimation>().StopAnimation();
            RhythmManager.Instance.isPlaying = false;
            return;
        }

        if (ghosts.Count > curIndex)
        {
            tempTime = Time.time - curTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckGhost();
                return;
            }

            if(tempTime - checkTimes[curIndex] > 0.4f) //너무 늦은 경우 실패 체크 위함
                CheckGhost();
        }
    }

    public void StartRhythmAction()
    {
        if (isPlaying) return;
        if (ghosts.Count == 0)
        {
            if (isReplay)
            {
                MakeGhost();
                isReplay = false;
            }
            else return;
        }
        SoundManager.Instance.PlaySfx(blockSound);

        ghostOriginal.AddComponent<GhostAnimation>();
        ghostOriginal.GetComponent<GhostAnimation>().animationSpeed = animMatch;

        ghostOriginal.GetComponent<GhostAnimation>().moving = direction.normalized * ghostGaps;
        ghostOriginal.GetComponent<GhostAnimation>().PlayAnimation();
        
        ghostOriginal.transform.position = playerTrans.position;
        ghostOriginal.transform.localRotation = Quaternion.Euler(rotateAngle);
        ghostOriginal.transform.forward = direction;
        curIndex = 0;
        isPlaying = true;
        curTime = Time.time;
    }

    void CheckGhost()
    {
        if (curIndex >= ghosts.Count)
        {
            return;
        }

        if (tempTime - checkTimes[curIndex] < -0.8f) //아주 빨리 쳤을 경우 뒤의 고스트 인식 하지 않도록    
            return;

        ghosts[curIndex].CheckGhost(tempTime - checkTimes[curIndex]);

        if (ghosts[curIndex].isOverGood)
        {
            RhythmManager rhythmManager = RhythmManager.Instance;
            float soundLevel = rhythmManager.curve.Evaluate(rhythmManager.curMusicTime / rhythmManager.totalMusicTime);
            int level = (int)(soundLevel * 5 - 0.0001f) + 1;
            if (level < 1) level = 1;
            if (level > 5) level = 5;
            hitSound[0] = "Note_N" + level;
            hitSound[1] = "Note_P" + level;

            //Debug.Log(hitSound[0]);

            if (hitSound[0] != null && hitSound[1] != null)
            {
                SoundManager.Instance.PlaySfx(ghosts[curIndex].isPointNotes ? hitSound[1] : hitSound[0]);
            }
        }
        curIndex++;

        if (curIndex < ghosts.Count) // 한 번 칠 때마다 외각선 오브젝트 위치 변경
        {
            if (ghostClip != null)
                ghostClip.SampleAnimation(ghostCurTiming, ghosts[curIndex].animTime);

            ghostCurTiming.transform.position = ghosts[curIndex].transform.position;
        }
        
    }

    public void SetBeatList(List<float> beats, List<bool> pointNoteList, float bpm)
    {
        if (ghosts.Count > 0)
        {
            RemoveGhost();
        }


        this.beats = beats;
        this.bpm = bpm;
        this.pointNoteList = pointNoteList;
        MakeGhost();
    }

    void MakeGhost()
    {
        Vector3 createPos = playerTrans.position;
        
        float realTime = 0f;
        Renderer[] render;

        for (int i = 0; i < beats.Count; i++) //시간 계산
        {
            float nextBeat = beats[i];
            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }

            if (bpm <= 0)
            {
                bpm = 120f; //default
            }

            realTime += (60f / bpm) / nextBeat;

            checkTimes.Add(realTime);
        }

        if (isAnimMatch)
            animMatch = ghostClip.length / checkTimes[checkTimes.Count - 1];
        else
            animMatch = 1f;

        for (int i = 0; i < beats.Count; i++) //고스트 생성
        {
            float nextBeat = beats[i];
            GameObject go = Instantiate(ghostOriginal, playerTrans);
            Ghost ghost = go.AddComponent<Ghost>();
            ghost.ghostIndex = i;
            if(go.TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = false;
            }
            
            ghost.isPointNotes = pointNoteList[i];

            if (ghostGaps != 0f)
            {
                createPos += playerTrans.forward.normalized * (ghostGaps * (60f / bpm) / nextBeat); 
                ghost.transform.position = createPos;
                ghost.transform.localRotation = Quaternion.Euler(rotateAngle);
            }

            if (ghostClip != null)
            {
                if (isAnimMatch)
                {
                    realTime = checkTimes[i] * animMatch;
                }
                else
                {
                    realTime = checkTimes[i];
                    while(true)
                    {
                        if (realTime < ghostClip.length)
                            break;
                        realTime -= ghostClip.length;
                    }
                }
                ghostClip.SampleAnimation(ghost.gameObject, realTime);
                ghost.animTime = realTime;
                
            }

            if (ghostMat != null) //고스트 머테리얼이 여부
            {
                render = ghost.GetComponentsInChildren<Renderer>(includeInactive: true);
                if (render != null)
                {
                    foreach (Renderer renderer in render)
                    {
                        Material[] mats = renderer.materials;

                        for (int j = 0; j < mats.Length; j++)
                            mats[j] = ghostMat;


                        renderer.materials = mats;
                    }
                }
            }

            ghosts.Add(ghost);
        }

        if (outlineMat != null) //아웃라인 머테리얼 여부
        {
            ghostCurTiming = Instantiate(ghostOriginal, playerTrans);

            if (ghostCurTiming.TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = false;
            }
            if (ghostClip != null)
            {
                ghostClip.SampleAnimation(ghostCurTiming, ghosts[0].animTime);
            }

            ghostCurTiming.transform.position = ghosts[0].transform.position;
            ghostCurTiming.transform.localRotation = Quaternion.Euler(rotateAngle);
            
            render = ghostCurTiming.GetComponentsInChildren<Renderer>(includeInactive: true);
            if (render != null)
            {
                foreach (Renderer renderer in render)
                {
                    Material[] mats = renderer.materials;

                    for (int j = 0; j < mats.Length; j++)
                        mats[j] = outlineMat;


                    renderer.materials = mats;
                }
            }
        }

        
        playerTrans.forward = direction;
        
    }

    public void RemoveGhost()
    {
        int idx = ghosts.Count;
        playerTrans.localRotation = Quaternion.identity;
        if (ghostCurTiming != null)
            Destroy(ghostCurTiming);

        for (int i = 0; i < idx; i++)
        {
            Destroy(ghosts[0].gameObject);
            ghosts.RemoveAt(0);
            checkTimes.RemoveAt(0);
        }
    }

}
