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
    public List<int> qtePosition; //qte가 생성될 위치 // row * col - 1 

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

    public bool isOverGood;

    private bool isAllNoteEnd;

    public bool isHolding; //롱노트 누르고 있는지의 여부
    public bool isLongNoteDoing; //롱노트 자체가 처리 중인지 확인

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

        isLongNoteDoing = false;
        //RhythmManager.Instance.rhythmActions.Add(this);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !(qteList.Count <= 0))
        {
            CheckQTE();
        }

        if(Input.GetKeyUp(KeyCode.Space) && isHolding)
        {
            //롱노트 처리
            isHolding = false;
            if(qteList.Count > 0 && qteList[0] is QTELong)
            {
                QTELongRelease();
            }
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
        QTE qte;
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

        if (qtePosition.Count < beats.Count)
        {
            qtePosition = new List<int>();
            for (int i = 0; i < beats.Count; i++)
                qtePosition.Add(-1);
        }

        for (int i = 0; i < beats.Count; i++)
        {
            float nextBeat = beats[i];

            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }

            if(isLongNoteDoing) //롱노트 처리 중엔 시간만 넘기기 //생성 X
            {
                if (isLongNote[i])
                {
                    isLongNoteDoing = false;
                    //isHolding = false;
                }

                yield return new WaitForSeconds((60f / bpm) / nextBeat);
                continue;
            }

            yield return new WaitForSeconds((60f / bpm) / nextBeat);
            if (isLongNote[i]) //롱노트 시작
            {
                qte = Instantiate(qteLongPrefabs, canvas.transform).GetComponent<QTELong>();

                //롱 노트 처리
                float holdingTime = 0f; 
                for(int j = i + 1; j < beats.Count; j++)
                {
                    holdingTime += (60f / bpm) / beats[j];
                    ((QTELong)qte).holdingCheckTime.Add(holdingTime);
                    if (isLongNote[j]) 
                        break;
                }

                ((QTELong)qte).holdingTime = holdingTime;
                isLongNoteDoing = true;
            }
            else //일반 노트
            {
                qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTEShort>();
            }

            qteList.Add(qte);
            if (qtePosition[i] == -1)
                randPos = Random.Range(0, row * col);
            else
                randPos = qtePosition[i];

            if (randPos >= row * col)
                randPos = randPos % (row * col);
            
            qte.transform.position = new Vector2(rootPositionX + (randPos % col) * gapX, rootPositionY + (randPos / col) * gapY);

            qte.manager = this;
            qte.isPointNotes = pointNoteList[i];
            
            if (bpm <= 0)
            {
                bpm = 120f; //default
            }
        }
        isAllNoteEnd = true;
    }

    public void CheckQTE()
    {
        if (qteList.Count <= 0)
        {
            return;
        }

        if (qteList[0] == null)
        {
            qteList.RemoveAt(0);
            return;
        }

        qteList[0].CheckJudge();

        if (hitSound[0] != null && hitSound[1] != null)
        {
            if (isOverGood)
                SoundManager.Instance.PlaySfx(qteList[0].isPointNotes ? hitSound[1] : hitSound[0]);
        }

        if ((qteList[0] is QTEShort) || !isHolding) //롱노트가 진행 중일 땐 제거 X
        {
            qteList.RemoveAt(0);
        }

        if(qteList.Count == 0 && isAllNoteEnd) 
            RhythmManager.Instance.isPlaying = false;
    }

    public void QTELongRelease() //롱노트 놓는 경우
    {
        if (qteList[0] is QTELong qte)
        {
            qte.ReleaseNote();
        }
    }

}
