using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class TimerController : MonoBehaviour
{
    [HideInInspector] public float totalTime = 180f;
    [HideInInspector] public float warningThreshold = 30f;
    [HideInInspector] public bool displayMinutes = true;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image timerFill;
    [SerializeField] private Image timerBackground;

    [SerializeField] private Color normalBGColor;
    [SerializeField] private Color timeoutBGColor;
    [SerializeField] private Color warningFGColor;
    [SerializeField] private Color normalFGColor;

    [SerializeField] private AudioClip end;
    [SerializeField] private AudioClip warning;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;

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
                audioSource.clip = warning;
                audioSource.Play();
                animator.SetTrigger("flash");
            }
            if (remainingTime <= 0f)
            {
                timerBackground.color = timeoutBGColor;

                remainingTime = 0f;
                isTimerRunning = false;

                pauseButton.SetActive(false);
                startButton.SetActive(false);
                resumeButton.SetActive(false);

                // Play sound
                audioSource.clip = end;
                audioSource.Play();
            }
        }
        if (displayMinutes)
        {
            string timeMin = Mathf.FloorToInt(remainingTime / 60f).ToString();
            string timeSec = Mathf.FloorToInt(remainingTime % 60f).ToString().Length > 1 ? Mathf.FloorToInt(remainingTime % 60f).ToString() : '0' + Mathf.FloorToInt(remainingTime % 60f).ToString();
            timerText.text = timeMin.ToString() + ':' + timeSec.ToString();
        }
        else
        {
            timerText.text = Mathf.Floor(remainingTime).ToString();
        }
        
        timerFill.fillAmount = remainingTime / totalTime;
    }

    public void StartTimer()
    {
        remainingTime = totalTime;
        isTimerRunning = true;

        pauseButton.SetActive(true);
        startButton.SetActive(false);
        resumeButton.SetActive(false);

        // Debug.Log("started timer");
    }
    public void StopTimer()
    {
        isTimerRunning = false;
        pauseButton.SetActive(false);
        startButton.SetActive(false);
        resumeButton.SetActive(true);

        // Debug.Log("stoped timer");
    }
    public void ResetTimer()
    {
        remainingTime = totalTime;
        isTimerRunning = false;

        timerFill.color = normalFGColor;
        timerBackground.color = normalBGColor;

        pauseButton.SetActive(false);
        startButton.SetActive(true);
        resumeButton.SetActive(false);

        // Debug.Log("reset timer");
    }
    public void ResumeTimer()
    {
        isTimerRunning = true;
        pauseButton.SetActive(true);
        startButton.SetActive(false);
        resumeButton.SetActive(false);

        // Debug.Log("resumed timer");
    }

}
