## Prefab Creation Plan

This document outlines the prefabs to create for the Portrait Demo scene and how they compose into the final scene.

Prefabs to Create
- PassengerGroup.prefab
  - Visual: A small group of colored spheres representing a group of passengers.
  - Components: ScriptableColorReference or Color enum exposure; GroupSize int; optional cooldown/spawn data.

- Lane.prefab
  - Visual: A lane area with windrow/queue visualization.
  - Children: LaneQueue holder (empty GameObject) to host PassengerGroup instances.
  - Components: LaneController (exposed color, capacity, spawn group size).

- WaitingRoomPanel.prefab
  - Visual: UI panel to display capacity and current occupancy visually.
  - Components: WaitingRoomController (capacity int, currentCount int).

- Bus.prefab
  - Visual: Color-coded bus body for each color (Red/Green/Blue).
  - Components: BusController (color, capacity, onboard list).

- RoutePath.prefab
  - Visual: A simple track/path (could be a 2D/3D path) with a pickup zone.
  - Components: RouteController (speed, stopIndex).

- PickupZone.prefab
  - Visual: Represents the bus stop location. May be a trigger collider.
  - Components: PickupZone marker (no logic, consumed by RouteController for boarding triggers).

- LevelConfigSO.asset (ScriptableObject)
  - Data: LevelNumber, WaitingCapacity, BusCapacity, MaxActiveBuses, BusSpeed, SpawnSpacing, etc.

How prefabs relate to scene
- PortraitDemo scene will instantiate and arrange instances of Lane.prefab for Red/Green/Blue lanes.
- WaitingRoomPanel.prefab will be placed in the UI canvas or overlay layer.
- Bus.prefab instances will be spawned on RoutePath at Start, with 2–3 instances active.
- RoutePath.prefab defines the motion loop; PickupZone is the boarding point.
- LevelConfigSO provides per-level tuning for waiting capacity, bus capacity, and speed.

Notes
- Use a single material/palette for all three colors; color is controlled via a color parameter or materialPropertyBlock to avoid material duplication.
- Keep prefabs modular for easy swapping and iteration.
- Document the property names in the corresponding components to ease handoff.
