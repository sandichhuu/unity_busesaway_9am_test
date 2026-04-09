using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int[] gridSize; // [width, height]
    public List<BusInfo> buses;
    public List<ExitInfo> exits;
    public List<List<int>> obstacles; // [x,y]
}

[System.Serializable]
public class BusInfo
{
    public string id;
    public string color;
    public int[] pos; // [x, y]
    public string dir;
}

[System.Serializable]
public class ExitInfo
{
    public int[] pos; // [x, y]
    public string color;
}

// LevelLoader: materializes the level from a JSON file in StreamingAssets/Levels/level1.json
public class LevelLoader : MonoBehaviour
{
    public GameObject busPrefab;
    public GameObject exitPrefab;
    public GameObject obstaclePrefab;
    public string levelFileName = "level1.json";

    private GridManager gridManager;
    private Dictionary<string, GameObject> busObjects = new Dictionary<string, GameObject>();

    void Start()
    {
        // Ensure a GridManager exists
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            var go = new GameObject("GridManager");
            gridManager = go.AddComponent<GridManager>();
        }

        // Ensure core managers exist
        EnsureCoreManagers();

        // If prefabs not assigned, create simple placeholders
        if (busPrefab == null) busPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (exitPrefab == null) exitPrefab = GameObject.CreatePrimitive(PrimitiveType.Plane);
        if (obstaclePrefab == null) obstaclePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Spawn the level
        SpawnLevel();
    }

    void SpawnLevel()
    {
    string path = Path.Combine(Application.streamingAssetsPath, "Levels", levelFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("Level file not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        // Grid size
        if (data.gridSize != null && data.gridSize.Length >= 2)
        {
            gridManager.width = data.gridSize[0];
            gridManager.height = data.gridSize[1];
        }

        // Level parent container
        GameObject levelParent = new GameObject("Level");

        // Spawn buses
        if (data.buses != null)
        {
            foreach (var b in data.buses)
            {
                Vector2Int pos = new Vector2Int(b.pos[0], b.pos[1]);
                Vector3 world = gridManager.GridToWorld(pos);
                GameObject bus = Instantiate(busPrefab, world, Quaternion.identity, levelParent.transform);
                bus.name = "Bus_" + b.id;
                var bc = bus.GetComponent<BusController>();
                if (bc == null) bc = bus.AddComponent<BusController>();
                bc.busId = b.id;
                bc.colorName = b.color;
                bc.gridPos = pos;
                busObjects[b.id] = bus;
                gridManager.SetOccupied(pos, true);
                // Register bus with GameManager for tracking
                var gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.RegisterBus(bc);
                }
            }
        }

        // Spawn obstacles
        if (data.obstacles != null)
        {
            foreach (var o in data.obstacles)
            {
                if (o.Count < 2) continue;
                Vector2Int pos = new Vector2Int(o[0], o[1]);
                Vector3 world = gridManager.GridToWorld(pos);
                Instantiate(obstaclePrefab, world, Quaternion.identity, levelParent.transform);
                gridManager.SetObstacle(pos, true);
            }
        }

        // Spawn exits
        if (data.exits != null)
        {
            foreach (var e in data.exits)
            {
                Vector2Int pos = new Vector2Int(e.pos[0], e.pos[1]);
                Vector3 world = gridManager.GridToWorld(pos);
                GameObject exit = Instantiate(exitPrefab, world, Quaternion.Euler(90, 0, 0), levelParent.transform);
                exit.name = "Exit_" + e.color;
                var r = exit.GetComponent<Renderer>();
                if (r != null) r.material.color = ColorFromName(e.color);
            }
        }
    }

    Color ColorFromName(string name)
    {
        switch (name?.ToLower())
        {
            case "red": return Color.red;
            case "blue": return Color.blue;
            case "green": return Color.green;
            case "yellow": return Color.yellow;
            default: return Color.white;
        }
    }

    void EnsureCoreManagers()
    {
        // GameManager
        if (FindObjectOfType<GameManager>() == null)
        {
            var gmGo = new GameObject("GameManager");
            gmGo.AddComponent<GameManager>();
        }
        // InputHandler
        if (FindObjectOfType<InputHandler>() == null)
        {
            var ihGo = new GameObject("InputHandler");
            ihGo.AddComponent<InputHandler>();
        }
    }
}
