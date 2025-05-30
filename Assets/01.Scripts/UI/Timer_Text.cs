using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Text : MonoBehaviour
{
    public TextMeshProUGUI timer;
    private float sec = 60f;
    private float currentTime;
    private bool isRunning = false;

    private void Start()
    {
        currentTime = sec;
    }

    private void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // 음수 방지
            if (currentTime < 0)
                currentTime = 0;

            Timer();
        }
        else
        {
            isRunning = false ;
        }
    }
    void Timer()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60); // 분 단위 계산
        int seconds = Mathf.FloorToInt(currentTime % 60); // 초 단위 계산

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer(float sec)
    {
        currentTime = sec;
        isRunning = true;
        Timer();
    }

}
