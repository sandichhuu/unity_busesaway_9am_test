# Passenger Entity

Objective
- Define the Passenger entity and its color state, spawn behavior, and queueing in lanes.

Data Model (C#-style skeleton)
```csharp
public enum PassengerColor { Red, Green, Yellow, Blue, Purple }

public class Passenger
{
    public int id;
    public PassengerColor Color;
    public Vector3 position; // for animation, or LocalPosition in a lane queue
}
```

Step-by-step Tasks
1. Create Passenger.cs with the above data model in your Scripts folder.
2. Expose a factory method to spawn a Passenger with a given color.
3. Create a small test harness to instantiate a few passengers of different colors.
4. Create a LaneQueue container (see Markdown/02_lane.md) that holds a List<Passenger>.
5. Ensure color identity is visible in the editor (via color swatch or enum display).

Acceptance Criteria
- Passenger class exists with Color property and integer id.
- A spawn mechanism can create a Passenger of a given color.
- LaneQueue is prepared to hold passengers and move them to a waiting area.
