# Waiting Room

Objective
- Represent the waiting room that accumulates boarded passengers and then hands them off to the bus when a color match occurs.

Data Model (C#-style skeleton)
```csharp
public class WaitingRoom
{
    public Dictionary<PassengerColor, List<Passenger>> colorBuckets = new Dictionary<PassengerColor, List<Passenger>>();
    public int total => colorBuckets.Values.Sum(l => l.Count);

    public void AddPassengers(PassengerColor color, IEnumerable<Passenger> passengers);
    public List<Passenger> TakePassengers(PassengerColor color, int maxCount);
}
```

Step-by-step Tasks
1. Create WaitingRoom.cs with a colorBuckets dictionary.
2. Implement AddPassengers and TakePassengers to manage color-based queues.
3. Expose a simple API: public int CountColor(PassengerColor color) and public List<Passenger> DrawColor(PassengerColor color, int max).
4. Integrate with Bus.AttemptBoard so that when a bus arrives, you call waitingRoom.TakePassengers(bus.Color, bus.Capacity - bus.OnBoard.Count) and transfer to bus.OnBoard.
5. Add basic UI hook to display color counts for debugging.

Acceptance Criteria
- Waiting room tracks per-color queues and total.
- Bus can draw matching-color passengers from waiting room up to remaining capacity.
