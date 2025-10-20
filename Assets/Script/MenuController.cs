using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public string defaultFileName = "save.json";
    private string defaultText =
    @"
{
  ""settings"": {
    ""pro_colors"": ""#0000FF"",
    ""con_colors"": ""#FF0000"",
    ""time_warning"": 30,
    ""time_prep"": 300,
    ""time_free"": 500,
    ""display_minutes"": true
  },
  ""title"": ""New Debate Title"",
  ""pro_side"": [
    {
      ""name"": ""Team 1 A"",
      ""time"": 240
    },
    {
      ""name"": ""Team 1 B"",
      ""time"": 180
    },
    {
      ""name"": ""Team 1 C"",
      ""time"": 180
    },
    {
      ""name"": ""Team 1 D"",
      ""time"": 300
    }
  ],
  ""con_side"": [
    {
      ""name"": ""Team 2 A"",
      ""time"": 240
    },
    {
      ""name"": ""Team 2 B"",
      ""time"": 180
    },
    {
      ""name"": ""Team 2 C"",
      ""time"": 180
    },
    {
      ""name"": ""Team 2 D"",
      ""time"": 300
    }
  ],
  ""event_order"": [
    ""prep"",
    1,
    -1,
    2,
    -2,
    3,
    -3,
    4,
    -4
  ]
}
    ";

    private string cachedText;
    private string path;

    private bool sceneLoaded = false;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text topic;
    [SerializeField] private TimerController simpleTimerController;
    [SerializeField] private GameObject proAnchor;
    [SerializeField] private GameObject conAnchor;
    [SerializeField] private GameObject timelineAnchor;
    [SerializeField] private GameObject speakerPrefab;
    [SerializeField] private GameObject timelineButtonPrefab;
    [SerializeField] private GameObject defaulTimer;
    [SerializeField] private GameObject doubleTimer;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private float totalSpace = 314f;
    [SerializeField] private float spaceBetween = 20f;

    public List<GameObject> proSpeakers = new List<GameObject>();
    public List<GameObject> conSpeakers = new List<GameObject>();

    public List<GameObject> timelineButtons = new List<GameObject>();

    public int currentEventIndex = 0;
    public DebateData debateData;

    private static int currentSpeakerID = int.MinValue;
    public static int CurrentSpeakerID { get { return currentSpeakerID; } }

    private static int nextSpeakerID = int.MinValue;
    public static int NextSpeakerID { get { return nextSpeakerID; } }

    private static bool prepPhase = true;
    public static bool PrepPhase { get { return prepPhase; } set { prepPhase = value; } }

    private static bool freePhase = false;
    public static bool FreePhase { get { return freePhase; } set { freePhase = value; } }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        if (!sceneLoaded)
        {
            loadScene();
            sceneLoaded = true;
        }

        cachedText = defaultText;
        inputField.text = LoadFileText();

    }

    private void OnDisable()
    {
        if (inputField.text.Length > 3)
            SaveToFile(inputField.text);
    }

    private void SaveToFile(string text)
    {
        path = Path.Combine(Application.persistentDataPath, defaultFileName);

        try
        {
            Debug.Log("Writing to file...");
            File.WriteAllText(path, text);
            Debug.Log($"File saved to: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save file: {e.Message}");
            inputField.text = $"Error: {e.Message}";
        }
    }
    
    private string LoadFileText()
    {
        path = Path.Combine(Application.persistentDataPath, defaultFileName);

        if (!File.Exists(path))
        {
            Debug.Log("Creating file...");
            SaveToFile(defaultText);
            return defaultText;
        }

        try
        {
            string text = File.ReadAllText(path);
            Debug.Log($"File loaded from: {path}\nContent: {text}");
            return text;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load file: {e.Message}");
            return $"Error: {e.Message}";
        }
    }

    public void ResetButton()
    {
        SaveToFile(defaultText);
        inputField.text = defaultText;
    }

    public void SaveButton()
    {
        SaveToFile(inputField.text);
    }

    public void DiscardButton()
    {
        inputField.text = cachedText;
    }

    public void QuitTimer()
    {
        Application.Quit();
    }

    public void Toggle()
    {
        panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf)
            cachedText = LoadFileText();
    }

    public void Next()
    {
        currentEventIndex++;

        // find next speaker
        if (currentEventIndex + 1 < debateData.event_order.Length)
        {
            int.TryParse(debateData.event_order[currentEventIndex + 1].ToString(), out nextSpeakerID);
        }
        else
        {
            nextSpeakerID = int.MinValue;
        }

        // Execute current event

        prepPhase = false;
        freePhase = false;

        defaulTimer.SetActive(true);
        doubleTimer.SetActive(false);

        if (currentEventIndex >= debateData.event_order.Length)
        {
            // End Debate
            currentEventIndex = 0;
            endScreen.SetActive(true);
        }
        else if (debateData.event_order[currentEventIndex] == "prep")
        {
            // Prep 
            simpleTimerController.totalTime = debateData.settings.time_prep;
            topic.text = "Preparation Time";
            currentSpeakerID = int.MinValue;
            prepPhase = true;

            simpleTimerController.ResetTimer();
            simpleTimerController.StartTimer();
        }
        else if (debateData.event_order[currentEventIndex] == "free")
        {
            // Free Debate
            topic.text = "Free Debate Time";
            currentSpeakerID = int.MinValue;
            freePhase = true;

            defaulTimer.SetActive(false);
            doubleTimer.SetActive(true);

            doubleTimer.GetComponent<InvertTimer>().setTimeSynced(debateData.settings.time_free);
        }
        else
        {
            int.TryParse(debateData.event_order[currentEventIndex].ToString(), out currentSpeakerID);

            if (currentSpeakerID > 0)
            {
                // Pro Speaker
                simpleTimerController.totalTime = debateData.pro_side[currentSpeakerID - 1].time;
                topic.text = $"Speaker {debateData.pro_side[currentSpeakerID - 1].name}'s Turn";

                simpleTimerController.ResetTimer();
                simpleTimerController.StartTimer();
            }
            else
            {
                // Con Speaker
                simpleTimerController.totalTime = debateData.con_side[currentSpeakerID * (-1) - 1].time;
                topic.text = $"Speaker {debateData.con_side[currentSpeakerID * (-1) - 1].name}'s Turn";

                simpleTimerController.ResetTimer();
                simpleTimerController.StartTimer();
            }

            // let animator handle the rest
        }

        Debug.Log($"Current Event Index: {currentEventIndex}, Current Speaker ID: {currentSpeakerID}, Next Speaker ID: {nextSpeakerID}");
    }

    public void loadScene()
    {
        string jsonString = LoadFileText();

        Debug.Log(jsonString);
        // Deserialize JSON to DebateData object
        debateData = JsonUtility.FromJson<DebateData>(jsonString);
        Debug.Log(debateData);

        topic.text = debateData.title;
        simpleTimerController.warningThreshold = debateData.settings.time_warning;
        simpleTimerController.displayMinutes = debateData.settings.display_minutes;

        // Spawn pro side speakers
        int numberOfPro = debateData.pro_side.Length;
        float size = (totalSpace - (numberOfPro - 1) * spaceBetween) / numberOfPro;

        for (int i = 1; i <= numberOfPro; i++)
        {
            GameObject obj;

            // Check if speaker already exists
            if (i - 1 < proSpeakers.Count && proSpeakers[i - 1] != null)
            {
                obj = proSpeakers[i - 1];
            }
            else             
            {
                obj = Instantiate(speakerPrefab, proAnchor.transform);
                proSpeakers.Add(obj);
            }

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * i * (size + spaceBetween));
            obj.GetComponent<RectTransform>().localScale = new Vector3(size/67f, size/67f * 0.8f, 1);
            obj.GetComponentInChildren<TMP_Text>().text = i.ToString();
            obj.GetComponent<AnimationController>().index = i;

            ColorUtility.TryParseHtmlString(debateData.settings.pro_colors, out Color proColor);
            obj.GetComponent<Image>().color = proColor;
        }

        // Remove extra speakers
        if (proSpeakers.Count > numberOfPro)
        {
            for (int i = numberOfPro; i < proSpeakers.Count; i++)
            {
                if (proSpeakers[i] != null)
                    proSpeakers[i].SetActive(false);
            }
        }

        // spawn con side speakers
        int numberOfCon = debateData.con_side.Length;
        size = (totalSpace - (numberOfCon - 1) * spaceBetween) / numberOfCon;

        for (int i = 1; i <= numberOfCon; i++)
        {
            GameObject obj;

            // Check if speaker already exists
            if (i - 1 < conSpeakers.Count && conSpeakers[i - 1] != null)
            {
                obj = conSpeakers[i - 1];
            }
            else
            {
                obj = Instantiate(speakerPrefab, conAnchor.transform);
                conSpeakers.Add(obj);
            }

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * i * (size + spaceBetween));
            obj.GetComponent<RectTransform>().localScale = new Vector3((-1) * size / 67f, size / 67f * 0.8f, 1);
            obj.GetComponentInChildren<TMP_Text>().transform.localScale = new Vector3(-1, 1, 1);
            obj.GetComponentInChildren<TMP_Text>().text = i.ToString();
            obj.GetComponent<AnimationController>().index = i * (-1);

            ColorUtility.TryParseHtmlString(debateData.settings.con_colors, out Color conColor);
            obj.GetComponent<Image>().color = conColor;
        }

        // Remove extra speakers
        if (conSpeakers.Count > numberOfCon)    
        {
            for (int i = numberOfCon; i < conSpeakers.Count; i++)
            {
                if (conSpeakers[i] != null)
                    conSpeakers[i].SetActive(false);
            }
        }

        // Spawn in timeline

        float buttonWidth = 30f;
        float startPostion = -300f;
        float distance = (650f - debateData.event_order.Length * buttonWidth) / debateData.event_order.Length;

        // disable all existing buttons

        for (int i = 0; i < timelineButtons.Count; i++)
        {
            timelineButtons[i].SetActive(false);
        }

        for (int i = 0; i < debateData.event_order.Length; i++)
        {
            GameObject obj;
            if (i < timelineButtons.Count && timelineButtons[i] != null)
            {
                obj = timelineButtons[i];
            }
            else
            {
                obj = Instantiate(timelineButtonPrefab, timelineAnchor.transform);
                timelineButtons.Add(obj);
            }

            obj.transform.localPosition = new Vector3(startPostion + i * (buttonWidth + distance), 0, 0);
            obj.GetComponent<TimelineButtonControl>().buttonEventIndex = i + 1;
            obj.SetActive(true);

            string displayText = debateData.event_order[i].ToString();

            if (displayText == "prep")
                displayText = "P";
            else if (displayText == "free")
                displayText = "F";

            obj.GetComponentInChildren<TMP_Text>().text = displayText;
        }

        currentEventIndex = -1;
    }
}
