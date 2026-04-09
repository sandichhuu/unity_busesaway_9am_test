# Game Controller

Objective
- Orchestrate the core loop: spawn waves, spawn bus, handle bus arrival, and update level progress.

Data Model (C#-style skeleton)
```csharp
public class GameController
{
    public LevelManager levelManager;
    public Spawner spawner;
    public Bus currentBus;
    public WaitingRoom waitingRoom;

    public void StartGame();
    public void OnBusArrived();
}
```

Step-by-step Tasks
1. Create GameController.cs to connect LevelManager, Spawner, Bus, and WaitingRoom.
2. Implement StartGame to initialize first level and begin spawning waves.
3. Implement OnBusArrived to trigger boarding and transition to the next phase (bus moves along track or disappears).
4. Wire up events between components: Spawner -> Lane -> WaitingRoom -> Bus.
5. Add basic UI updates (level, progress bar, and bus status).

Acceptance Criteria
- GameController coordinates wave spawning, bus spawning, and boarding flow without stalls.
