using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    public float bpm = 120.0f; //120 bpm
    public float[] beat = new float[2] { 1f, 2f }; //박자

    public GameObject qtePrefabs;
    public Canvas canvas;

    public List<QTE> qteList;

    private AudioSource audioSource;
    private AudioClip beepClip;


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
        audioSource = gameObject.AddComponent<AudioSource>();
        beepClip = CreateBeepClip();

        qteList = new List<QTE>();
        StartCoroutine(MakeQTE());
        InvokeRepeating(nameof(PlayBeep), 0f, 60f / bpm);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckQTE();
        }
    }

    IEnumerator MakeQTE()
    {
        while(true)
        {
            QTE qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTE>();
            qteList.Add(qte);
            float nextBeat = beat[Random.Range(0, beat.Length)];
 
            yield return new WaitForSeconds(60f/bpm/nextBeat);
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
    void PlayBeep()
    {
        audioSource.PlayOneShot(beepClip);
    }

    AudioClip CreateBeepClip(float frequency = 880f, float duration = 0.05f)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip clip = AudioClip.Create("Beep", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
