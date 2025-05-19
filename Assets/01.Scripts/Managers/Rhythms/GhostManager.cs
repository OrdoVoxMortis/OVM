using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour, IRhythmActions
{
    public Transform playerTrans; //고스트 시작 위치
    public Vector3 direction; //고스트가 생성되는 방향

    public Vector3 rotateAngle; //고스트가 생성된 후 회전각도

    public float ghostGaps;

    public float bpm;
    public List<float> beats;    //입력 받을 패턴 리스트
    public List<bool> pointNoteList; //true인 경우 point note

    public List<Ghost> ghosts;
    public List<float> checkTimes; //나중에 ghost로 이동 

    public GameObject ghostPrefabs;
    public AnimationClip ghostClip;

    public float curTime;
    public bool isPlaying;
    private int curIndex = 0;

    public string[] hitSound = new string[2]; //0은 일반 노트 //1은 포인트 노트
    public string blockSound;

    private GameObject ghostCurTiming;

    private bool isReplay;
    private float tempTime;
    public Material ghostMat;
    public Material outlineMat;

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
            ghostPrefabs.GetComponent<GhostAnimation>().StopAnimation();
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

        ghostPrefabs.AddComponent<GhostAnimation>().PlayAnimation();
        SoundManager.Instance.PlaySfx(blockSound);
        ghostPrefabs.GetComponent<GhostAnimation>().moving = direction.normalized * ghostGaps;
        ghostPrefabs.transform.position = playerTrans.position;
        ghostPrefabs.transform.forward = direction;
        ghostPrefabs.transform.rotation = Quaternion.Euler(rotateAngle);
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

            Debug.Log(hitSound[0]);

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
        float time = 0f;
        float realTime = 0f;
        Renderer render;
        for (int i = 0; i < beats.Count; i++)
        {
            float nextBeat = beats[i];
            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }
            
            if(bpm <= 0)
            {
                bpm = 120f; //default
            }


            GameObject go = Instantiate(ghostPrefabs, playerTrans);
            Ghost ghost = go.AddComponent<Ghost>();
            
            ghost.isPointNotes = pointNoteList[i];

            if (ghostGaps != 0f)
            {
                createPos += playerTrans.forward.normalized * (ghostGaps * (60f / bpm) / nextBeat);
                ghost.transform.position = createPos;
                ghost.transform.rotation = Quaternion.Euler(rotateAngle);
            }

            if (ghostClip != null)
            {
                time += (60f / bpm) / nextBeat;
                realTime += (60f / bpm) / nextBeat;
                while (true)
                {
                    if (time < ghostClip.length)
                        break;
                    time -= ghostClip.length;
                }
                ghostClip.SampleAnimation(ghost.gameObject, time);
                ghost.animTime = time;
                
            }

            if (ghostMat != null) //고스트 머테리얼이 여부
            {
                render = ghost.GetComponent<Renderer>();
                if(render == null)
                    render = ghost.GetComponentInChildren<Renderer>(includeInactive: true);
                Material[] mats = render.materials;

                for (int j = 0; j < mats.Length; j++)
                    mats[j] = ghostMat;
                

                render.materials = mats;
            }

            ghosts.Add(ghost);
            checkTimes.Add(realTime);
        }


        if (outlineMat != null) //아웃라인 머테리얼 여부
        {
            ghostCurTiming = Instantiate(ghostPrefabs, playerTrans);


            if(ghostClip != null)
                ghostClip.SampleAnimation(ghostCurTiming, checkTimes[0]);

            ghostCurTiming.transform.position = ghosts[0].transform.position;
            ghostCurTiming.transform.rotation = Quaternion.Euler(rotateAngle);

            render = ghostCurTiming.GetComponent<Renderer>();
            if (render == null)
                render = ghostCurTiming.GetComponentInChildren<Renderer>(includeInactive: true);

            Material[] mats;
            if (render != null)
            {
                mats = render.materials;

                for (int i = 0; i < mats.Length; i++)
                    mats[i] = outlineMat;

                render.materials = mats;
            }
        }

        
        playerTrans.forward = direction;
        
    }

    public void RemoveGhost()
    {
        int idx = ghosts.Count;
        playerTrans.rotation = Quaternion.identity;
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
