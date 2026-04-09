# Portrait Demo Folder

This folder contains design docs, prefab plans, and script structure for the Portrait screen of the Buses Away demo.

What’s inside
- Phases.md: Plan to implement the base / core game logic.
- Scene.md: Portrait scene object layout and responsibilities.
- PrefabPlan.md: Plan for creating and wiring prefabs used in the demo.
- ScriptStructure.md: Proposed script organization and responsibilities.
- README.md: How to use this folder and how to integrate with the project.

How to use
- Use Scene.md to understand object roles and relationships for the MVP portrait scene.
- Follow PrefabPlan.md to create and organize prefabs in Unity. Keep changes data-driven where possible.
- Implement ScriptStructure.md guidance in code organization. Start with models/controllers in MVP form.
- Read README.md for a quickstart on opening and using this folder within your Unity project.

How to start (quickstart)
- Implement prefabs per PrefabPlan.md and wire them to the scene according to Scene.md.
- Create and assign LevelConfig assets for Level 1 and Level 2.
- Run the scene in Unity to verify the core loop: 3 lanes feeding a Waiting Room and 2–3 buses on a loop.
