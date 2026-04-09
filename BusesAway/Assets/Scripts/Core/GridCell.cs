using UnityEngine;
using BusesAway.Bus;

namespace BusesAway.Core
{
    public class GridCell
    {
        public Vector2Int Position { get; set; }
        public TileType Type { get; set; }
        public BusController OccupiedBus { get; set; }
        public ExitType Exit { get; set; }

        public GridCell(Vector2Int position)
        {
            Position = position;
            Type = TileType.Empty;
            OccupiedBus = null;
            Exit = null;
        }

        public bool IsOccupied()
        {
            return OccupiedBus != null || Type == TileType.Obstacle;
        }

        public bool IsWalkable()
        {
            return Type == TileType.Empty || Type == TileType.Exit;
        }
    }

    public class ExitType
    {
        public Vector2Int Position { get; set; }
        public BusColor RequiredColor { get; set; }
    }
}
