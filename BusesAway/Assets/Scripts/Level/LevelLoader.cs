using UnityEngine;
using System.IO;
using BusesAway.Grid;
using BusesAway.Bus;
using BusesAway.Core;
using BusesAway.Managers;

namespace BusesAway.Level
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private string levelFileName = "level1";
        [SerializeField] private GameObject busPrefab;
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private Transform levelParent;

        private void Start()
        {
            LoadLevel(levelFileName);
        }

        public void LoadLevel(string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Levels", fileName + ".json");

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Level file not found: {filePath}. Creating default level.");
                CreateDefaultLevel();
                return;
            }

            string json = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            if (levelData != null)
            {
                BuildLevel(levelData);
            }
        }

        public void LoadLevelFromJson(string json)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);
            if (levelData != null)
            {
                BuildLevel(levelData);
            }
        }

        private void BuildLevel(LevelData data)
        {
            ClearLevel();

            Vector2Int gridSize = data.GetGridSize();
            GridManager gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager != null)
            {
                gridManager.InitializeGrid(gridSize.x, gridSize.y);
            }

            if (data.obstacles != null)
            {
                foreach (var obstacle in data.obstacles)
                {
                    Vector2Int pos = LevelDataParser.ParsePosition(obstacle);
                    SpawnObstacle(pos);
                }
            }

            if (data.exits != null)
            {
                foreach (var exit in data.exits)
                {
                    Vector2Int pos = LevelDataParser.ParsePosition(exit.pos);
                    BusColor color = LevelDataParser.ParseBusColor(exit.color);
                    SpawnExit(pos, color);
                }
            }

            if (data.buses != null)
            {
                foreach (var busData in data.buses)
                {
                    Vector2Int pos = LevelDataParser.ParsePosition(busData.pos);
                    BusColor color = LevelDataParser.ParseBusColor(busData.color);
                    Direction dir = LevelDataParser.ParseDirection(busData.dir);
                    SpawnBus(busData.id, color, pos, dir);
                }
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.InitializeLevel();
            }
        }

        private void SpawnBus(string id, BusColor color, Vector2Int pos, Direction dir)
        {
            if (busPrefab == null) return;

            GridManager gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null) return;

            GameObject busObj = Instantiate(busPrefab, levelParent);
            busObj.transform.position = gridManager.GridToWorld(pos) + Vector3.up * 0.5f;

            BusController bus = busObj.GetComponent<BusController>();
            if (bus != null)
            {
                bus.Initialize(id, color, pos, dir);
            }

            GridCell cell = gridManager.GetCell(pos);
            if (cell != null)
            {
                cell.Type = TileType.Bus;
                cell.OccupiedBus = bus;
            }
        }

        private void SpawnExit(Vector2Int pos, BusColor color)
        {
            if (exitPrefab == null) return;

            GridManager gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null) return;

            GameObject exitObj = Instantiate(exitPrefab, levelParent);
            exitObj.transform.position = gridManager.GridToWorld(pos) + Vector3.up * 0.1f;

            Exit exit = exitObj.GetComponent<Exit>();
            if (exit != null)
            {
                exit.Initialize(color, pos);
            }

            gridManager.SetExit(pos, color);
        }

        private void SpawnObstacle(Vector2Int pos)
        {
            if (obstaclePrefab == null) return;

            GridManager gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null) return;

            GameObject obstacleObj = Instantiate(obstaclePrefab, levelParent);
            obstacleObj.transform.position = gridManager.GridToWorld(pos) + Vector3.up * 0.5f;

            GridCell cell = gridManager.GetCell(pos);
            if (cell != null)
            {
                cell.Type = TileType.Obstacle;
            }
        }

        private void ClearLevel()
        {
            if (levelParent != null)
            {
                while (levelParent.childCount > 0)
                {
                    DestroyImmediate(levelParent.GetChild(0).gameObject);
                }
            }
        }

        private void CreateDefaultLevel()
        {
            string defaultJson = @"
            {
                'gridSize': [6, 6],
                'buses': [
                    {'id':'b1','color':'Red','pos':[1,2],'dir':'Right'},
                    {'id':'b2','color':'Blue','pos':[4,1],'dir':'Down'},
                    {'id':'b3','color':'Green','pos':[2,4],'dir':'Up'}
                ],
                'exits': [
                    {'pos':[5,2],'color':'Red'},
                    {'pos':[4,5],'color':'Blue'},
                    {'pos':[2,0],'color':'Green'}
                ],
                'obstacles':[[3,2],[3,3],[1,4]]
            }";

            defaultJson = defaultJson.Replace("'", "\"");
            LoadLevelFromJson(defaultJson);
        }
    }
}
