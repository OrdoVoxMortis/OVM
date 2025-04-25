using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour, IRhythmActions
{

    public float bpm = 120.0f; //120 bpm
    public List<float> beats;    //입력 받을 패턴 리스트
    //beat로 들어오는 값 1 = 4분음표

    public List<QTE> qteList; //처리할 QTE
    public List<bool> pointNoteList; //true인 경우 point note

    public GameObject qtePrefabs;
    public Canvas canvas;

    public AudioClip[] hitSound = new AudioClip[2]; //0은 일반 노트 //1은 포인트 노트
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        qteList = new List<QTE>();
        audioSource = gameObject.AddComponent<AudioSource>();

        //오디오 클립가져오기
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !(qteList.Count <= 0))
        {
            CheckQTE();
        }
    }

    public void SetBeatList(List<float> beats, List<bool> pointNoteList, float bpm)
    {
        this.beats = beats;
        this.pointNoteList = pointNoteList;
        this.bpm = bpm;
    }

    public void StartRhythmAction()
    {
        //RhythmManager.Instance.qteManager = this;
        StartCoroutine(MakeQTE());
        Debug.Log("qte 생성!");
    }

    IEnumerator MakeQTE()
    {
        for (int i = 0; i < beats.Count; i++)
        {
            QTE qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTE>();
            qteList.Add(qte);
            qte.manager = this;
            qte.isPointNotes = pointNoteList[i];
            float nextBeat = beats[i];
            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }

            if (bpm <= 0)
            {
                bpm = 120f; //default
            }

            yield return new WaitForSeconds((60f / bpm) / nextBeat);
        }
        RhythmManager.Instance.isPlaying = false;
    }

    public void CheckQTE()
    {
        if (qteList.Count <= 0)
        {
            return;
        }

        if (hitSound[0] == null || hitSound[1] == null)
        {
            audioSource.PlayOneShot(qteList[0].isPointNotes ? hitSound[1] : hitSound[0]);
        }

        qteList[0].CheckJudge();
        qteList.RemoveAt(0);
    }

}
