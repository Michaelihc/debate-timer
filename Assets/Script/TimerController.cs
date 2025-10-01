using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class TimerController : MonoBehaviour
{
    public float totalTime = 180f;
    public float warningThreshold = 30f;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image timerFill;
    [SerializeField] private Image timerBackground;

    [SerializeField] private Color normalBGColor;
    [SerializeField] private Color timeoutBGColor;
    [SerializeField] private Color warningFGColor;
    [SerializeField] private Color normalFGColor;

    [SerializeField] private AudioClip end;
    [SerializeField] private AudioClip warning;
    [SerializeField] private AudioSource audio;
    [SerializeField] private Animator animator;

    private float remainingTime = 0f;
    public float RemainingTime { get { return remainingTime; } }

    private bool isTimerRunning = false;
    public bool IsTimerRunning { get { return isTimerRunning; } }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= warningThreshold && remainingTime + Time.deltaTime > warningThreshold)
            {
                timerFill.color = warningFGColor;
                // Play warning sound
                audio.clip = warning;
                audio.Play();
                animator.SetTrigger("flash");
            }
            if (remainingTime <= 0f)
            {
                timerBackground.color = timeoutBGColor;

                remainingTime = 0f;
                isTimerRunning = false;
                
                // Play sound
                audio.clip = end;
                audio.Play();
            }
        }
        timerText.text = Mathf.Ceil(remainingTime).ToString();
        timerFill.fillAmount = remainingTime / totalTime;
    }

    public void StartTimer()
    {
        remainingTime = totalTime;
        isTimerRunning = true;
    }
    public void StopTimer()
    {
        isTimerRunning = false;
    }
    public void ResetTimer()
    {
        remainingTime = totalTime;
        isTimerRunning = false;

        timerFill.color = normalFGColor;
        timerBackground.color = normalBGColor;
    }
    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

}
