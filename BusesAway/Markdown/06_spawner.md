# Spawner

Objective
- Spawn waves: colors, bus, and timing for each level.

Data Model (C#-style skeleton)
```csharp
public class Spawner
{
    public float SpawnIntervalSeconds = 2.0f;
    public PassengerColor NextColor;
    public void SpawnWave(); // spawn passengers of NextColor into 3 lanes
}
```

Step-by-step Tasks
1. Create Spawner.cs with a timer that calls SpawnWave at intervals.
2. Implement SpawnWave to push a wave of Passengers of NextColor into the three lanes (randomized lane distribution).
3. After each spawn, rotate NextColor to the next color in a fixed order or cycle through colors.
4. Ensure integration with Markdown/01_passenger.md and Markdown/02_lane.md by using Lane.Enqueue().
5. Add debug logs for wave spawning counts per color.

Acceptance Criteria
- Waves spawn passengers of a single color across the lanes at the specified interval.
- Lane queues reflect newly spawned passengers.
