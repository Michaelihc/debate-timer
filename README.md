# Debate Timer
A GUI timer specifically designed for debates, with indicators on current and next speaker. Built in Unity.

# Features
- Visualized debate layout
- Current and next speaker indicator
- Double timer for free debate
- Timeline
- Customizable colors

<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/bc08d26b-cc9b-4412-81c9-37081f76b472" />
<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/30eb5090-94d4-4bd2-b3bc-61fc373af34f" />
<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/350b44da-efd2-4fd3-b854-ac01be6de10e" />
<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/5ad19ad9-7381-438e-9a71-925d0c3eaf18" />
<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/3f9daa7a-a802-478e-bf78-f10f70a533fb" />
<img width="2880" height="1920" alt="image" src="https://github.com/user-attachments/assets/95b3ebe1-70d0-4bb3-bafc-09df5730d0c3" />

# Template JSON

```json
{
  "settings": {
    "pro_colors": "#0000FF",
    "con_colors": "#FF0000",
    "time_warning": 30,
    "time_prep": 300,
    "time_free": 500,
    "display_minutes": true
  },
  "title": "New Debate Title",
  "pro_side": [
    {
      "name": "Team 1 A",
      "time": 240
    },
    {
      "name": "Team 1 B",
      "time": 180
    },
    {
      "name": "Team 1 C",
      "time": 180
    },
    {
      "name": "Team 1 D",
      "time": 300
    }
  ],
  "con_side": [
    {
      "name": "Team 2 A",
      "time": 240
    },
    {
      "name": "Team 2 B",
      "time": 180
    },
    {
      "name": "Team 2 C",
      "time": 180
    },
    {
      "name": "Team 2 D",
      "time": 300
    }
  ],
  "event_order": [
    "prep",
    1,
    -1,
    2,
    -2,
    3,
    -3,
    4,
    -4,
    "prep",
    "free"
  ]
}
```

# ToDo
1. ~~Double timer for free debate~~
2. Labels for For and Against side
3. Custom Background Colors
4. Custom Audio for timers
5. GUI settings menu (instead of JSON)
6. Clean UI
7. Remove unused shaders
