## Script Structure

This document outlines the intended C# script structure for the Portrait Demo scene, including namespaces, folders, and core responsibilities.

Folder structure (Assets)
- Scripts/Models
  - Color.cs
  - Passenger.cs
  - PassengerGroup.cs
  - LevelConfig.cs (if not ScriptableObject directly)

- Scripts/Controllers
  - LaneController.cs (manages a single lane's queue and taps)
  - WaitingRoomController.cs (enforces capacity, holds passengers waiting to board)
  - BusController.cs (manages onboard passengers and boarding logic)
  - RouteController.cs (moves buses along the fixed path, triggers stops)
  - LevelManager.cs (loads LevelConfig, handles progression)
  - InputHandler.cs (handles tap detection on lanes and UI actions)
  - PrefabBinder.cs (optional helper to bind prefabs to scene objects at runtime)

- Scripts/Services
  - ConfigLoader.cs (loads LevelConfig from JSON or ScriptableObjects)
  - SceneInitializer.cs (bootstraps scene objects and references)

- Scripts/UI
  - HUDController.cs (updates WaitingRoom capacity, active buses, level indicator)
  - MenuController.cs (pause/restart, settings) - optional for MVP

- Scripts/Utils
  - ColorExtensions.cs (utility for mapping Color enum to actual Color values or materials)
  - SceneUtils.cs (helpers for scene dimension checks)

Data types
- Enum: BusColor (Red, Green, Blue)
- Class/Struct: Passenger
- Class: PassengerGroup
- Class: LevelConfig (ScriptableObject or JSON payload)

Notes
- All scripts should be simple, with clear public APIs for other components to call (e.g., EnqueueGroup, BoardFromWaitingRoom, IsFull).
- Use ScriptableObject LevelConfig for editor-driven tuning; provide JSON fallback if needed for runtime configurability.
- Keep IO isolated in ConfigLoader; avoid direct dependencies on scene logic in data models.
