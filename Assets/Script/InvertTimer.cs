using UnityEngine;

public class InvertTimer : MonoBehaviour
{
    [SerializeField] private TimerController timer1;
    [SerializeField] private GameObject timer1Label;

    [SerializeField] private TimerController timer2;
    [SerializeField] private GameObject timer2Label;

    private void Update()
    {
        if (timer1.IsTimerRunning)
        {
            timer1Label.SetActive(true);
        }
        else
        {
            timer1Label.SetActive(false);
        }

        if (timer2.IsTimerRunning)
        {
            timer2Label.SetActive(true);
        }
        else
        {
            timer2Label.SetActive(false);
        }
    }

    public void swap()
    {
        if (timer1.IsTimerRunning && !timer2.IsTimerRunning)
        {
            timer1.StopTimer();
            timer2.ResumeTimer();
        }
        else if (timer2.IsTimerRunning && !timer1.IsTimerRunning)
        {
            timer2.StopTimer();
            timer1.ResumeTimer();
        }
    }

    public void setTimeSynced(float time)
    {
        timer1.totalTime = time;
        timer1.ResetTimer();
        timer2.totalTime = time;
        timer2.ResetTimer();
    }    
}
