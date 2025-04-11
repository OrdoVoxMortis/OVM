using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    public Transform playerTrans;
    public Vector3 startPos;  //고스트의 시작 위치
    public Vector3 direction; //고스트가 생성되는 방향

    public float bpm;
    public List<float> beats;    //입력 받을 패턴 리스트

    public List<Ghost> ghosts;

    public GameObject ghostPrefabs;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        ghosts = new List<Ghost>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CheckGhost();
        }
    }

    void CheckGhost()
    {

    }

    public void SetBeatList(List<float> beats, float bpm)
    {
        this.beats = beats;
        this.bpm = bpm;

        MakeGhost();
    }

    void MakeGhost()
    {
        Vector3 createPos = startPos;
        for (int i = 0; i < beats.Count; i++)
        {
            float nextBeat = beats[i];

            Ghost ghost = Instantiate(ghostPrefabs).GetComponent<Ghost>();
            ghosts.Add(ghost);
            ghost.transform.position = createPos; 
            createPos += direction * 10f * (60f / bpm) / nextBeat; //중간의 상수는 물체나 플레이어의 움직임 속도

            if (nextBeat <= 0)
            {
                nextBeat = 1;
            }
        }
    }
}
