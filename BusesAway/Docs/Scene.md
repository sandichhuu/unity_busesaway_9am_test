# Portrait Screen Scene Description

This document describes the key GameObjects and their role in the Portrait screen (demo scene) for Buses Away.

Scene Root
- PortraitRoot (Empty): Parent for all portrait UI and world elements.
- MainCamera: Shows the 2D/3D view of the portrait scene.
- UICamera (optional): Renders UI on top of the scene (if separate camera is used).

World Elements
- Background: Static backdrop for the portrait screen (colors/textures aligned with the palette).
- LevelBar (UI): A progress indicator showing the current level progress (Level 1 or Level 2).

Lanes and Waiting Room
- Lane_Red: A lane stack holding red passengers awaiting transfer to the Waiting Room.
- Lane_Green: A lane stack holding green passengers awaiting transfer to the Waiting Room.
- Lane_Blue: A lane stack holding blue passengers awaiting transfer to the Waiting Room.
- WaitingRoomPanel: UI panel showing the Waiting Room capacity and current occupancy.
- WaitingRoomGrid (optional): Visual grid or area showing passengers currently in the Waiting Room.

Bus Route and Pick-up
- RoutePath: Visual representation of the fixed track along which buses move.
- Bus_1, Bus_2, Bus_3: Active buses on the track (2–3 at a time) with color-coded bodies.
- PickupZone: The stop area where buses halt and board colored passengers from the Waiting Room.
- TunnelExit: Visual cue for buses disappearing once they fill and depart.

UI and HUD
- PauseButton: Gear icon to pause the demo.
- RestartButton: Circular arrow to reset the scene.
- CoinDisplay/Score (optional): Top-right indicator showing current score or currency.

Notes
- All color handling is restricted to the three demo colors: Red, Green, Blue.
- The Waiting Room capacity is configured per level (Phase 4 Task definitions).
- Buses board passengers by color match, and depart when full; they disappear after exiting the tunnel.

Graph/Relationships
- PortraitRoot -> [Background, LevelBar, Lanes, WaitingRoomPanel, RoutePath, Buses]
- Lane_Red/Green/Blue -> WaitingRoomPanel (enqueue on tap)
- Bus_1..3 -> RoutePath -> PickupZone (boarding) -> TunnelExit (despawn)

This scene should be lightweight and data-driven, enabling quick iteration of the core gameplay loop without heavy visuals.
