using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostManager : MonoBehaviour, IRhythmActions
{
    public Transform playerTrans; //고스트 시작 위치
    public Vector3 direction; //고스트가 생성되는 방향

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
   

    private float tempTime;

    // Start is called before the first frame update
    void Start()
    {
        ghosts = new List<Ghost>();
        checkTimes = new List<float>();
        curIndex = 0;
        isPlaying = false;
        hitSound[0] = "Note_N1";
        hitSound[1] = "Note_P1";
    

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

        if(ghosts.Count == 0) return;

        ghostPrefabs.AddComponent<GhostAnimation>().PlayAnimation();
        SoundManager.Instance.PlaySfx(blockSound);
        ghostPrefabs.GetComponent<GhostAnimation>().moving = direction.normalized * ghostGaps;
        ghostPrefabs.transform.position = playerTrans.position;
        ghostPrefabs.transform.forward = direction;
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

        ghosts[curIndex].CheckGhost(tempTime - checkTimes[curIndex]);

        if (ghosts[curIndex].isOverGood)
        {
            if (hitSound[0] != null && hitSound[1] != null)
            {
                SoundManager.Instance.PlaySfx(ghosts[curIndex].isPointNotes ? hitSound[1] : hitSound[0]);
            }
        }
        
        curIndex++;
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
                createPos += playerTrans.forward.normalized * ghostGaps * (60f / bpm) / nextBeat;
                ghost.transform.position = createPos;
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
                
            }

            ghosts.Add(ghost);
            checkTimes.Add(realTime);
        }

        playerTrans.forward = direction;
        
    }

    public void RemoveGhost()
    {
        int idx = ghosts.Count;
        for (int i = 0; i < idx; i++)
        {
            Destroy(ghosts[0].gameObject);
            ghosts.RemoveAt(0);
            checkTimes.RemoveAt(0);
        }
    }

   

}
