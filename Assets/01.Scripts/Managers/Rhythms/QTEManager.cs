using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    public float bpm = 120.0f; //120 bpm
    public List<float> beats;    //입력 받을 패턴 리스트
    //beat로 들어오는 값 1 = 4분음표

    public List<QTE> qteList; //처리할 QTE

    public GameObject qtePrefabs;
    public Canvas canvas;

    private AudioSource audioSource;
    public AudioClip beepClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        qteList = new List<QTE>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !(qteList.Count <= 0))
        {
            audioSource.PlayOneShot(beepClip);
            CheckQTE();
        }
    }

    public void SetBeatList(List<float> beats, float bpm)
    {
        this.beats = beats;
        this.bpm = bpm;

        StartCoroutine(MakeQTE());
    }

    IEnumerator MakeQTE()
    {
        for (int i = 0; i < beats.Count; i++)
        {
            QTE qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTE>();
            qteList.Add(qte);
            float nextBeat = beats[i];
            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }

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
        qteList.RemoveAt(0);
    }

}
