using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public int index;

    private Animator animator;
    private TimerController timerController;
    [SerializeField] private GameObject clipBoard;
    [SerializeField] private GameObject groupBubble;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        timerController = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuController.PrepPhase)
        {
            clipBoard.SetActive(true);
        }
        else
        {
            clipBoard.SetActive(false);
        }

        if (MenuController.FreePhase)
        {
            groupBubble.SetActive(true);
        }
        else
        {
            groupBubble.SetActive(false);
        }

        if (MenuController.CurrentSpeakerID == index)
        {
            animator.SetBool("Next", false);
            animator.SetBool("Next Flash", false);

            if (timerController.IsTimerRunning)
                animator.SetBool("Speak", true);
            else
                animator.SetBool("Speak", false);
        }
        else if (MenuController.NextSpeakerID == index)
        {
            animator.SetBool("Speak", false);

            if (timerController.RemainingTime < 0.1f)
            {
                animator.SetBool("Next", true);
                animator.SetBool("Next Flash", true);
            }
            else
            {
                animator.SetBool("Next", true);
                animator.SetBool("Next Flash", false);
            }
        }
        else
        {
            animator.SetBool("Speak", false);
            animator.SetBool("Next", false);
            animator.SetBool("Next Flash", false);
        }
    }
}
