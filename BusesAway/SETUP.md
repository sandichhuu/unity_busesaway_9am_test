# Buses Away - Unity Setup Guide

## Scene Setup

### 1. Create Main Scene
1. Create a new scene (File > New Scene)
2. Save it as `Assets/Scenes/GameScene.unity`

### 2. Setup Camera
1. Select Main Camera
2. Set Position to: `(2.5, 10, 2.5)`
3. Set Rotation to: `(90, 0, 0)` (for top-down view)
4. Set Projection to: `Orthographic`
5. Set Size to: `5`

### 3. Create Game Objects

Create the following hierarchy:

```
GameScene
├── Directional Light
├── Main Camera
├── Managers
│   └── (Add components: GameManager, MovementManager, GridManager)
├── LevelLoader
│   └── (Add component: LevelLoader)
├── Input
│   └── (Add component: InputHandler)
└── Level
    └── (This will hold spawned level objects)
```

#### GridManager Settings
- Width: `6`
- Height: `6`
- Cell Size: `1`

#### LevelLoader Settings
- Level File Name: `level1`
- Bus Prefab: Drag Bus prefab here
- Exit Prefab: Drag Exit prefab here
- Obstacle Prefab: Drag Obstacle prefab here
- Level Parent: Drag the "Level" GameObject here

#### InputHandler Settings
- Swipe Threshold: `50`
- Bus Layer: Create and assign "Bus" layer

## Prefab Setup

### 1. Bus Prefab

Create a cube and add:

**Components:**
- Transform
- Mesh Renderer (for body)
- Box Collider
- BusController script

**Visual Setup:**
1. Create a Cube named "Body"
2. Create a smaller cube/arrow named "DirectionIndicator" as child
3. Position direction indicator at front of bus
4. Assign to BusController script fields

**Collider Setup:**
- Set Box Collider size to cover the bus
- Set layer to "Bus"

### 2. Exit Prefab

Create a plane and add:

**Components:**
- Transform
- Mesh Renderer
- Exit script

**Visual:**
- Scale to match cell size (1x1)
- Position at y=0.1

### 3. Obstacle Prefab

Create a cube and add:

**Components:**
- Transform
- Mesh Renderer
- Box Collider

**Visual:**
- Use a dark material
- Scale slightly smaller than cell

## Layer Configuration

1. Go to Edit > Project Settings > Tags and Layers
2. Add new layer:
   - User Layer 6: `Bus`
3. Set InputHandler's Bus Layer to the Bus layer

## Level JSON Format

Place level files in `Assets/StreamingAssets/Levels/`

Example structure:

```json
{
  "gridSize": [6, 6],
  "buses": [
    {"id":"b1","color":"Red","pos":[1,2],"dir":"Right"},
    {"id":"b2","color":"Blue","pos":[4,1],"dir":"Down"},
    {"id":"b3","color":"Green","pos":[2,4],"dir":"Up"}
  ],
  "exits": [
    {"pos":[5,2],"color":"Red"},
    {"pos":[4,5],"color":"Blue"},
    {"pos":[2,0],"color":"Green"}
  ],
  "obstacles":[[3,2],[3,3],[1,4]]
}
```

### Field Descriptions

**buses:**
- `id`: Unique identifier for the bus
- `color`: Red, Blue, Green, or Yellow
- `pos`: [x, y] grid position
- `dir`: Up, Down, Left, or Right

**exits:**
- `pos`: [x, y] grid position
- `color`: Must match bus color

**obstacles:**
- Array of [x, y] positions for obstacles

## Quick Test

1. Save the scene
2. Press Play
3. Select a bus by clicking on it
4. Swipe in the direction the bus can move
5. Guide all buses to their matching color exits to win!

## Troubleshooting

- **Buses not spawning**: Check LevelLoader references and JSON file path
- **Buses not moving**: Ensure InputHandler has Bus layer set
- **Grid not visible**: Check GridManager cell size and grid dimensions
- **Camera view wrong**: Reset camera to orthographic, position (2.5, 10, 2.5), rotation (90, 0, 0)
