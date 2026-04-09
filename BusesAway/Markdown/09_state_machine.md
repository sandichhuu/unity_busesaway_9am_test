# State Machine (Gameplay Flow)

Objective
- Define a lightweight state machine to govern transitions: SpawnWaiting, BusArriving, Boarding, BusDeparting, LevelComplete.

States
- Idle
- Spawning
- BusArriving
- Boarding
- BusDeparting
- LevelComplete

Transitions (example)
- SpawnWaiting -> BusArriving when bus is spawned.
- BusArriving -> Boarding when bus reaches waiting room.
- Boarding -> BusDeparting when bus is full or wave ends.
- BusDeparting -> LevelComplete when level end condition met.
- LevelComplete -> SpawnWaiting (next wave/level) or end game.

Step-by-step Tasks
1. Create a simple finite state machine (State enum + Update() loop) to manage transitions.
2. Implement guards and events for transitions (e.g., OnBusFull, OnWaveCleared).
3. Integrate with existing components (Spawner, Bus, WaitingRoom, LevelManager).
4. Add unit tests or simple playtests to ensure transitions occur as expected.

Acceptance Criteria
- Clear state machine governs core gameplay loop with deterministic transitions.
