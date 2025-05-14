using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour, IRhythmActions
{

    public float bpm = 120.0f; //120 bpm
    public List<float> beats;    //입력 받을 패턴 리스트
    //beat로 들어오는 값 1 = 4분음표
    public List<bool> pointNoteList; //true인 경우 point note
    public List<bool> isLongNote; //true인 경우 long note

    public List<QTE> qteList; //처리할 QTE

    public GameObject qtePrefabs;
    public GameObject qteLongPrefabs;
    public Canvas canvas;
    public string eventSound;

    public string[] hitSound = new string[2]; //0은 일반 노트 //1은 포인트 노트

    public string eventBgm;

    [Header("QTE 생성 위치 조절")]
    public int rootPositionX;
    public int rootPositionY;

    public int gapX;
    public int gapY;

    public int row;
    public int col;

    private int randPos; //0 ~ row * col - 1

    private bool isAllNoteEnd;

    // Start is called before the first frame update
    void Start()
    {
        qteList = new List<QTE>();
        hitSound[0] = "Note_N1";
        hitSound[1] = "Note_P1";
        
        if (row == 0)
            row = 8;
        if (col == 0)
            col = 6;

        //RhythmManager.Instance.rhythmActions.Add(this);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !(qteList.Count <= 0))
        {
            CheckQTE();
        }
    }

    public void SetBeatList(List<float> beats, List<bool> pointNoteList = null, float bpm = 120f, List<bool> isLongNote = null)
    {
        this.beats = beats;
        this.pointNoteList = pointNoteList;
        this.bpm = bpm;
        this.isLongNote = isLongNote;
    }

    public void StartRhythmAction()
    {
        //RhythmManager.Instance.qteManager = this;
        StartCoroutine(MakeQTE());
    }

    IEnumerator MakeQTE()
    {
        UI_QTE qteUI = UIManager.Instance.ShowUI<UI_QTE>("QTE_UI");
        qteUI.transform.SetAsFirstSibling();
        RhythmManager.Instance.checkJudgeText.transform.SetAsLastSibling();

        SoundManager.Instance.PlaySfx("eventBgm");
        isAllNoteEnd = false;

        if (pointNoteList.Count < beats.Count)
        {
            pointNoteList = new List<bool>();
            for (int i = 0; i < beats.Count; i++)
                pointNoteList.Add(false);
        }

        if (isLongNote.Count < beats.Count)
        {
            isLongNote = new List<bool>();
            for (int i = 0; i < beats.Count; i++)
                isLongNote.Add(false);
        }

        for (int i = 0; i < beats.Count; i++)
        {
            float nextBeat = beats[i];

            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }

            if (isLongNote[i])
            {
                //롱 노트 처리
                yield return new WaitForSeconds((60f / bpm) / nextBeat);
                continue;
            }

            QTEShort qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTEShort>();
            qteList.Add(qte);
            randPos = Random.Range(0, row * col);
            
            qte.transform.position = new Vector2(rootPositionX + (randPos % col) * gapX, rootPositionY + (randPos / col) * gapY);

            qte.manager = this;
            qte.isPointNotes = pointNoteList[i];
            
            if (bpm <= 0)
            {
                bpm = 120f; //default
            }

            if(i == beats.Count - 1)
                isAllNoteEnd = true;

            yield return new WaitForSeconds((60f / bpm) / nextBeat);
        }
        
    }

    public void CheckQTE()
    {
        if (qteList.Count <= 0)
        {
            return;
        }
        qteList[0].CheckJudge();

        if (hitSound[0] != null && hitSound[1] != null)
        {
            if (qteList[0].isOverGood)
                SoundManager.Instance.PlaySfx(qteList[0].isPointNotes ? hitSound[1] : hitSound[0]);
        }

        qteList.RemoveAt(0);
        if(qteList.Count == 0 && isAllNoteEnd) 
            RhythmManager.Instance.isPlaying = false;
    }

}
