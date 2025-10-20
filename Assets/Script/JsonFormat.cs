using System;
using UnityEngine;

// Root class for the entire JSON
[Serializable]
public class DebateData
{
    public Settings settings;
    public string title;
    public Participant[] pro_side;
    public Participant[] con_side;
    public string[] event_order;
}

// Settings class
[Serializable]
public class Settings
{
    public string pro_colors;
    public string con_colors;
    public int time_warning;
    public int time_prep;
    public int time_free;
    public bool display_minutes;
}

// Participant class for pro_side and con_side
[Serializable]
public class Participant
{
    public string name;
    public int time;
}