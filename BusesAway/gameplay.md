Buses Away – Unity Implementation Guide
1. Overview

Inspired by: Buses Away
Genre: Puzzle / Casual
Engine: Unity (C#)

Core Gameplay Loop
Player selects a bus
Player moves the bus (drag/swipe)
Bus moves if path is clear
Bus exits if conditions are correct
Repeat until win or deadlock
2. Project Architecture
2.1 Folder Structure
Assets/
├── Scripts/
│   ├── Core/
│   ├── Grid/
│   ├── Bus/
│   ├── Input/
│   ├── Level/
│   └── Managers/
├── Prefabs/
├── ScriptableObjects/
├── Scenes/
└── Resources/
3. Core Systems
3.1 Grid System
Responsibilities
Manage board state
Validate movement
Store tile data
Data Structure
public enum TileType {
    Empty,
    Bus,
    Obstacle,
    Exit
}

public class GridCell {
    public Vector2Int Position;
    public TileType Type;
    public Bus OccupiedBus;
}
Grid Manager
public class GridManager : MonoBehaviour {
    public int width;
    public int height;
    private GridCell[,] grid;

    public GridCell GetCell(Vector2Int pos) {
        return grid[pos.x, pos.y];
    }

    public bool IsInsideGrid(Vector2Int pos) {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }
}
3.2 Bus System
Bus Data
public enum BusColor {
    Red,
    Blue,
    Green,
    Yellow
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public class Bus : MonoBehaviour {
    public string id;
    public BusColor color;
    public Vector2Int position;
    public Direction direction;
}
3.3 Movement Logic
Rules
Bus only moves in its allowed direction
Cannot pass through other buses or obstacles
Can enter Exit tile
Movement Check
public bool CanMove(Bus bus, Direction dir) {
    Vector2Int nextPos = GetNextPosition(bus.position, dir);

    if (!gridManager.IsInsideGrid(nextPos))
        return false;

    var cell = gridManager.GetCell(nextPos);

    return cell.Type == TileType.Empty || cell.Type == TileType.Exit;
}
Move Execution
public void MoveBus(Bus bus, Direction dir) {
    if (!CanMove(bus, dir)) return;

    Vector2Int nextPos = GetNextPosition(bus.position, dir);

    UpdateGrid(bus.position, nextPos, bus);

    bus.position = nextPos;
    bus.transform.position = GridToWorld(nextPos);
}
3.4 Exit System
public class Exit : MonoBehaviour {
    public BusColor requiredColor;
    public Vector2Int position;
}
Exit Logic
public void TryExit(Bus bus, GridCell cell) {
    Exit exit = cell.GetComponent<Exit>();

    if (exit != null && exit.requiredColor == bus.color) {
        RemoveBus(bus);
    }
}
3.5 Input System
Approach

Use:

Mouse (Editor)
Touch (Mobile)
Basic Input
public class InputHandler : MonoBehaviour {
    private Bus selectedBus;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SelectBus();
        }

        if (Input.GetMouseButtonUp(0)) {
            TryMove();
        }
    }
}
Suggestion
Detect swipe direction
Convert to Direction
4. Game State Management
public class GameManager : MonoBehaviour {
    public List<Bus> buses;
    public int movesCount;

    public void CheckWin() {
        if (buses.Count == 0) {
            Debug.Log("WIN");
        }
    }

    public void CheckDeadlock() {
        foreach (var bus in buses) {
            if (HasValidMove(bus)) return;
        }

        Debug.Log("LOSE");
    }
}
5. Level System
JSON Format
{
  "gridSize": [6,6],
  "buses": [
    {"id":"b1","color":"Red","pos":[2,2],"dir":"Right"}
  ],
  "exits": [
    {"pos":[5,2],"color":"Red"}
  ],
  "obstacles":[[1,1],[1,2]]
}
Level Loader
[System.Serializable]
public class LevelData {
    public Vector2Int gridSize;
    public List<BusData> buses;
    public List<ExitData> exits;
    public List<Vector2Int> obstacles;
}
6. Win / Lose Conditions
Win
All buses exited correctly
Lose
No valid moves left (deadlock)
7. Animation (Recommended)
Movement: Lerp (0.2s)
Exit:
Scale down OR move out of screen
Invalid move:
Shake effect
8. Suggested Prefabs
BusPrefab
GridCellPrefab
ExitPrefab
ObstaclePrefab
9. Extension Features (Optional)
Undo system (stack of moves)
Hint system (basic solver)
Move limit per level
Level progression
10. Implementation Notes for Claude

When generating code:

Use clean architecture
Separate logic & MonoBehaviour
Avoid hardcoding values
Use ScriptableObject for configs
Make systems modular:
GridManager
BusController
InputHandler
GameManager
11. Minimal Implementation Order
Grid system
Bus spawning
Movement logic
Input handling
Exit logic
Win/Lose
Polish (animation, UI)
12. Deliverables

Claude should output:

C# scripts
Scene setup instructions
Prefab setup guide
Example level JSON