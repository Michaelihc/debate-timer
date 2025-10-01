using UnityEngine;

public class TimelineButtonControl : MonoBehaviour
{
    public int buttonEventIndex;
    private MenuController menuController;

    void Start()
    {
        menuController = GameObject.Find("Main Script").GetComponent<MenuController>();
    }

    public void OnButtonPress()
    {
        menuController.currentEventIndex = buttonEventIndex - 2;
        menuController.Next();
    }

    private void Update()
    {
        if (menuController.currentEventIndex == buttonEventIndex - 1)
            gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        else
            gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
}
