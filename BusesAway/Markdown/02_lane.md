# Lane (Queue) System

Objective
- Implement three lanes at the bottom where passengers queue and can be raised to the waiting room with a tap.

Data Model (C#-style skeleton)
```csharp
public class Lane
{
    public int laneIndex; // 0,1,2
    public List<Passenger> waitingQueue = new List<Passenger>();
    public void Enqueue(Passenger p) { waitingQueue.Add(p); }
    public List<Passenger> MoveToWaitingRoom(); // implement elsewhere
}
```

Step-by-step Tasks
1. Create Lane.cs with a laneIndex and a List<Passenger> waitingQueue.
2. Implement Enqueue and a MoveToWaitingRoom method stub that returns the current queue and clears it.
3. Create a LaneManager (could be a simple MonoBehaviour) that holds 3 Lane instances and a UI button area for tapping lanes.
4. Implement a TapHandler for each lane to trigger MoveToWaitingRoom on the associated Lane.
5. Validate spawning workflow from Markdown/01_passenger.md to ensure colours align with the next bus color (see Markdown/03_bus.md).

Acceptance Criteria
- Three Lane instances exist with independent queues.
- Tapping a lane moves all queued passengers to the waiting area (return value from MoveToWaitingRoom is non-null).
