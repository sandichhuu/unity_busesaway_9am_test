# Scene Setup Guide

Objective
- Provide a concrete, editor-friendly guide to assemble the scene that supports the core gameplay flow (lanes, waiting room, bus, tunnel, UI, and level scaffolding).

Scene Layout (concept)
- Bottom area: three lanes (Lane 0, Lane 1, Lane 2) where passengers queue.
- Center-bottom: waiting room area visually connected to lanes.
- Track: winding path that leads to a waiting room and a tunnel exit.
- Spawn points: Passenger spawner area at the bottom; Bus spawn area before the track.
- End-of-track tunnel: exit to disappear when conditions are met.
- UI: Level indicator, progress bar, lane taps, and color-counts.

Required Prefabs and GameObjects
- Prefabs (create under Assets/Prefabs):
  - PassengerPrefab: Visual passenger with a color indicator.
  - LanePrefab: Lane container with a trigger area and a display of the queue (optional).
  - BusPrefab: Bus with a color selector and capacity indicator.
  - WaitingRoomPrefab: Visual waiting area that can host boarded passengers.
  - LevelUIPrefab: UI for level title, progress bar, and currency if needed.
- Scene-scoped GameObjects (place under a root SceneRoot):
  - Track: A simple curved mesh or spline-based path.
  - BusSpawnPoint: Position where buses spawn.
  - PassengerSpawnPoint: Position at the bottom for spawning passengers.
  - TunnelExit: Point where buses disappear when not full and pass the waiting area.
  - UI Canvas: Parent for Level UI and other HUD elements.

Component wiring (required scripts)
- Passenger (Scripts/Passenger.cs): color, id
- Lane (Scripts/Lane.cs): laneIndex, waitingQueue, Enqueue()
- Bus (Scripts/Bus.cs): color, capacity, OnBoard, AttemptBoard()
- WaitingRoom (Scripts/WaitingRoom.cs): colorBuckets, AddPassengers(), TakePassengers()
- LevelManager (Scripts/LevelManager.cs): level data, progress
- Spawner (Scripts/Spawner.cs): color cycle, SpawnWave()
- GameController (Scripts/GameController.cs): orchestrates the flow
- UIManager (Scripts/UIManager.cs): updates level, colors, bus status
- Optional: StateMachine (Scripts/StateMachine.cs): governs core states

Scene Setup Steps (step-by-step)
1. Create a new scene: BusesAway_Scene. Save under Assets/Scenes.
2. Create a root object: SceneRoot (empty) and organize the hierarchy:
   - SceneRoot
     - Track (with a visible path)"
     - BusSpawnPoint
     - PassengerSpawnPoint
     - LaneRoot
       - Lane0 (Lane component, laneIndex=0)
       - Lane1 (Lane component, laneIndex=1)
       - Lane2 (Lane component, laneIndex=2)
     - WaitingRoom
     - TunnelExit
     - UI (Canvas) with LevelUI, ColorCounts, etc.
3. Create and assign prefabs:
   - PassengerPrefab: set up with a color property (use material or sprite tint to reflect color).
   - LanePrefab: lightweight container for lane visuals; instantiate 3 times as children of LaneRoot or use simple transforms.
   - BusPrefab: color property to reflect bus color; ensure capacity is visible if desired.
   - WaitingRoomPrefab: a simple visual panel showing queues.
4. Scene wiring (Inspector references):
   - SceneSetup or GameController references to the spawner, level manager, waiting room, and UI.
   - Assign Prefab references for PassengerPrefab, BusPrefab, etc., to enable runtime spawning.
5. Create a minimal main camera setup and an orthographic/ perspective view suitable for mobile.
6. Add a basic EventSystem and touch input support if not present.
7. Ensure the initial level loads and a first wave spawns by enabling GameController at Start.

Playback & Verification
- Run the scene and verify: taps on lanes move queue to waiting room; bus spawns; bus color matches first wave; boarding works; bus disappears when not full after tunnel reach.
- Confirm UI shows Level 1 progress and color counts.

Notes
- This guide aims to let you scaffold a working scene quickly. You can replace placeholders with your existing architecture, naming, and assets.
