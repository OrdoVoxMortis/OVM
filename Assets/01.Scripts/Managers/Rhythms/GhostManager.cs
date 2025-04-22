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

    public List<Ghost> ghosts;
    public List<float> checkTimes;

    public GameObject ghostPrefabs;
    public AnimationClip ghostClip;

    public float curTime;
    public bool isPlaying;
    private int curIndex = 0;


    float tempTime;

    // Start is called before the first frame update
    void Start()
    {
        ghosts = new List<Ghost>();
        checkTimes = new List<float>();
        curIndex = 0;
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        if (ghosts.Count > curIndex)
        {
            tempTime = Time.time - curTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckGhost();
                return;
            }

            if(tempTime - checkTimes[curIndex] > 0.1f) //너무 늦은 경우 실패 체크 위함
                CheckGhost();
        }
    }

    public void StartRhythmAction()
    {
        if (isPlaying) return;

        if(ghosts.Count == 0) return;

        ghostPrefabs.AddComponent<GhostAnimation>().PlayAnimation();
        ghostPrefabs.GetComponent<GhostAnimation>().moving = direction.normalized * ghostGaps;
        ghostPrefabs.transform.position = playerTrans.position;
        curIndex = 0;
        isPlaying = true;
        curTime = Time.time;
    }

    void CheckGhost()
    {
        if (curIndex >= ghosts.Count)
        {
            isPlaying = false;
            ghostPrefabs.AddComponent<GhostAnimation>().StopAnimation();
            return;
        }


        if (tempTime - checkTimes[curIndex] < 0.2f && tempTime - checkTimes[curIndex] > -0.2f)
        {
            Debug.Log("성공");
        } 
        else
        {
            Debug.Log("실패");
        }

        curIndex++;
    }

    public void SetBeatList(List<float> beats, float bpm)
    {
        if (ghosts.Count > 0)
        {
            RemoveGhost();
        }

        this.beats = beats;
        this.bpm = bpm;

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
