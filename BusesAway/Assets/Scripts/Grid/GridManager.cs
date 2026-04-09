using UnityEngine;
using BusesAway.Core;

namespace BusesAway.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int width = 6;
        [SerializeField] private int height = 6;
        [SerializeField] private float cellSize = 1f;

        private GridCell[,] grid;

        public int Width => width;
        public int Height => height;
        public float CellSize => cellSize;

        private void Awake()
        {
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            grid = new GridCell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new GridCell(new Vector2Int(x, y));
                }
            }
        }

        public void InitializeGrid(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            InitializeGrid();
        }

        public GridCell GetCell(Vector2Int pos)
        {
            if (!IsInsideGrid(pos))
                return null;
            return grid[pos.x, pos.y];
        }

        public GridCell GetCell(int x, int y)
        {
            return GetCell(new Vector2Int(x, y));
        }

        public bool IsInsideGrid(Vector2Int pos)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x / cellSize),
                Mathf.RoundToInt(worldPos.z / cellSize)
            );
        }

        public void SetCellType(Vector2Int pos, TileType type)
        {
            var cell = GetCell(pos);
            if (cell != null)
            {
                cell.Type = type;
            }
        }

        public void ClearGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new GridCell(new Vector2Int(x, y));
                }
            }
        }

        public void SetExit(Vector2Int pos, BusColor requiredColor)
        {
            var cell = GetCell(pos);
            if (cell != null)
            {
                cell.Type = TileType.Exit;
                cell.Exit = new ExitType
                {
                    Position = pos,
                    RequiredColor = requiredColor
                };
            }
        }

        private void OnDrawGizmos()
        {
            if (grid == null) return;

            Gizmos.color = Color.gray;
            for (int x = 0; x <= width; x++)
            {
                Vector3 start = new Vector3(x * cellSize - cellSize * 0.5f, 0, -cellSize * 0.5f);
                Vector3 end = new Vector3(x * cellSize - cellSize * 0.5f, 0, height * cellSize - cellSize * 0.5f);
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= height; y++)
            {
                Vector3 start = new Vector3(-cellSize * 0.5f, 0, y * cellSize - cellSize * 0.5f);
                Vector3 end = new Vector3(width * cellSize - cellSize * 0.5f, 0, y * cellSize - cellSize * 0.5f);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
