using UnityEngine;

// Centralized movement logic for buses on the grid.
public static class MovementManager
{
    private static GridManager _grid;
    private static GridManager Grid
    {
        get
        {
            if (_grid == null)
            {
                _grid = Object.FindObjectOfType<GridManager>();
            }
            return _grid;
        }
    }

    public static bool TryMove(BusController bus, string dir)
    {
        if (bus == null || Grid == null) return false;
        Vector2Int current = bus.gridPos;
        Vector2Int next = current;
        switch (dir)
        {
            case "Right": next = new Vector2Int(current.x + 1, current.y); break;
            case "Left": next = new Vector2Int(current.x - 1, current.y); break;
            case "Up": next = new Vector2Int(current.x, current.y + 1); break;
            case "Down": next = new Vector2Int(current.x, current.y - 1); break;
            default: return false;
        }
        if (!Grid.InBounds(next)) return false;
        if (!Grid.IsCellFree(next)) return false;

        // Move: update occupancy grid and bus position
        Grid.SetOccupied(current, false);
        Grid.SetOccupied(next, true);
        bus.gridPos = next;
        if (bus != null)
        {
            bus.transform.position = Grid.GridToWorld(next);
        }
        return true;
    }
}
