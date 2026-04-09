using UnityEngine;

// Lightweight grid manager for the setup.
public class GridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public float cellSize = 1f;

    // Occupancy and obstacles tracking
    public bool[,] occupancy;
    public bool[,] obstacles;

    void Awake()
    {
        occupancy = new bool[width, height];
        obstacles = new bool[width, height];
    }

    public Vector3 GridToWorld(Vector2Int pos)
    {
        return new Vector3(pos.x * cellSize, 0f, pos.y * cellSize);
    }

    public bool InBounds(Vector2Int p)
    {
        return p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
    }

    public bool IsCellFree(Vector2Int p)
    {
        if (!InBounds(p)) return false;
        if (occupancy[p.x, p.y]) return false;
        if (obstacles[p.x, p.y]) return false;
        return true;
    }

    public void SetOccupied(Vector2Int p, bool value)
    {
        if (InBounds(p)) occupancy[p.x, p.y] = value;
    }

    public void SetObstacle(Vector2Int p, bool value)
    {
        if (InBounds(p)) obstacles[p.x, p.y] = value;
    }
}
