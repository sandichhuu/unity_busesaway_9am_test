- Task 0.3: Data-driven LevelConfig schema (ScriptableObject or JSON)
  - Description: Create LevelConfig structure: levelNumber, waitingCapacity, busCapacity, maxActiveBuses, busSpeed, etc.
  - Acceptance: LevelConfig can be loaded for Level 1 and Level 2.
  - Owner: DataTeam

### Phase 1 — Core Data Scaffolding
- Task 1.1: Implement Lane data structure and 3 lanes with color associations
- Task 1.2: Implement WaitingRoom with capacity check
- Task 1.3: Implement Bus model with color, capacity, onboard list
- Task 1.4: Implement simple Route model (fixed path) and pickup stop

### Phase 2 — Lane Taps and WaitingRoom Integration
- Task 2.1: Tap handler to move front group from lane to WaitingRoom
- Task 2.2: Group size policy (fixed group size per tap)
- Task 2.3: Overflow detection and game-over handling

### Phase 3 — Bus Movement and Boarding
- Task 3.1: Spawn 2–3 buses on track (initial pool) and animate along path
- Task 3.2: Stop logic at pickup zone and color-matched boarding
- Task 3.3: Bus departure when full; clean removal from active list; spawn new bus if needed to maintain count

### Phase 4 — Win/Lose and Level Progression
- Task 4.1: Win condition: all lanes empty and WaitingRoom empty
- Task 4.2: Lose condition: overflow triggers Game Over
- Task 4.3: Linear progression: Level 1 → Level 2 with respective LevelConfig
- Task 4.4: Reset flow for replaying levels

### Phase 5 — Minimal UI and Debugging
- Task 5.1: HUD indicators (WaitingRoom capacity, active buses, level)
- Task 5.2: Debug toggles and log statements to verify flow
- Task 5.3: Basic pause/resume and restart controls

### Phase 6 — Testing Plan
- Task 6.1: Unit tests for core logic (boarding, enqueue, overflow)
- Task 6.2: Integration tests for full loop with 2 buses
- Task 6.3: Manual playthrough scenarios for win/lose

### Phase 7 — Polish and Data Tweaks
- Task 7.1: Wire ScriptableObject/JSON loading for LevelConfig in editor/build
- Task 7.2: Prepare Level 2 config values
- Task 7.3: Prepare a minimal QA pass

### Phase 8 — Deliverables
- Task 8.1: Patch notes and a minimal readme describing how to run the demo
- Task 8.2: A reproducible Level 1/Level 2 scriptable data bundle
