# Level Manager

Objective
- Drive level progression, track level number, and manage level-based progress/requirements.

Data Model (C#-style skeleton)
```csharp
public class LevelManager
{
    public int CurrentLevel = 1;
    public int TotalLevels = 100;
    public float LevelProgress; // 0..1
    public void CompleteLevel();
}
```

Step-by-step Tasks
1. Create LevelManager.cs with fields for CurrentLevel, TotalLevels, and LevelProgress.
2. Implement CompleteLevel() to advance CurrentLevel, reset progress, and trigger wave/spawn logic for the next level.
3. Hook LevelProgress to a UI progress bar (see Markdown/07_ui.md for UI hooks).
4. Add a simple save/load of CurrentLevel to PlayerPrefs (or your chosen save system).
5. Add end-of-level reward/transition visuals (optional).

Acceptance Criteria
- Level advances linearly, progress resets between levels, and next wave spawns after level complete.
