# Bus (Carrier) Object

Objective
- Define the Bus color, capacity, and boarding logic. The bus should collect passengers from the waiting room when colors match.

Data Model (C#-style skeleton)
```csharp
public class Bus
{
    public PassengerColor Color;
    public int Capacity;
    public List<Passenger> OnBoard = new List<Passenger>();

    public void AttemptBoard(Passenger p);
    public bool IsFull => OnBoard.Count >= Capacity;
}
```

Step-by-step Tasks
1. Create Bus.cs with Color, Capacity, and OnBoard list.
2. Implement AttemptBoard(Passenger p) that only boards if p.Color == Color and there is capacity.
3. Create a BusSpawner that instantiates a Bus at the start of a level, with a color matching the next wave or a specific color rule.
4. Implement the bus movement logic stub: Move along track, stop at waiting room, and then continue or disappear when conditions are met (full or not).
5. Integrate color matching so that only matching-colored passengers board when bus reaches waiting room.

Acceptance Criteria
- Bus can board passengers of matching color up to its capacity.
- When full, bus should continue; when not full at the end, it disappears after reaching tunnel (define behavior clearly in your game loop).
