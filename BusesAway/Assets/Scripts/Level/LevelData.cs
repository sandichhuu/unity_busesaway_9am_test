using System;
using System.Collections.Generic;
using UnityEngine;
using BusesAway.Core;

namespace BusesAway.Level
{
    [Serializable]
    public class BusData
    {
        public string id;
        public string color;
        public int[] pos;
        public string dir;
    }

    [Serializable]
    public class ExitData
    {
        public int[] pos;
        public string color;
    }

    [Serializable]
    public class LevelData
    {
        public int[] gridSize;
        public List<BusData> buses;
        public List<ExitData> exits;
        public List<int[]> obstacles;

        public Vector2Int GetGridSize()
        {
            if (gridSize != null && gridSize.Length >= 2)
            {
                return new Vector2Int(gridSize[0], gridSize[1]);
            }
            return new Vector2Int(6, 6);
        }
    }

    public static class LevelDataParser
    {
        public static BusColor ParseBusColor(string color)
        {
            if (Enum.TryParse<BusColor>(color, true, out BusColor result))
            {
                return result;
            }
            return BusColor.Red;
        }

        public static Direction ParseDirection(string dir)
        {
            if (Enum.TryParse<Direction>(dir, true, out Direction result))
            {
                return result;
            }
            return Direction.Right;
        }

        public static Vector2Int ParsePosition(int[] pos)
        {
            if (pos != null && pos.Length >= 2)
            {
                return new Vector2Int(pos[0], pos[1]);
            }
            return Vector2Int.zero;
        }
    }
}
