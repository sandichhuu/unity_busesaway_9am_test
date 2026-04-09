# UI Manager

Objective
- Drive user-facing UI: level progress, lanes tap indicators, color counts, bus status, and end-of-wave messages.

Data Model (C#-style skeleton)
```csharp
public class UIManager
{
    public void UpdateLevelProgress(float t); // 0..1
    public void UpdateLaneHint(int laneIndex, bool show);
    public void UpdateColorCounts(Dictionary<PassengerColor, int> counts);
    public void ShowBusStatus(string status);
}
```

Step-by-step Tasks
1. Create UIManager.cs with methods to update level progress, lane hints, color counts, and bus status.
2. Bind UI elements in the scene to these methods (e.g., sliders, small panels above lanes).
3. Implement a simple color legend in the UI for accessibility (color + pattern or icon).
4. Wire UI updates from LevelManager and Spawner to reflect current state.

Acceptance Criteria
- UI reflects level progress, lane states, and bus events in real-time.
