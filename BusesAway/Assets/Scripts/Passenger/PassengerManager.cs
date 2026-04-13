using BA.Data;
using BA.Lane;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BA.Passenger
{
    [Serializable]
    public class PassengerManager
    {
        [SerializeField] private PassengerFactory factory;

        public void SpawnPassengerBlock(LaneBehaviour lane, List<PassengerBlockData> spawnQueue)
        {
            var passengerBlocks = lane.GetPassengerBlocks();
            var grid = lane.GetGrid();
            var spawnCount = 0;
            for (var i = 0; i < spawnQueue.Count; i++)
            {
                var block = spawnQueue[i];
                if (block.amount + spawnCount >= grid.GetLength())
                    break;

                var passengers = new List<PassengerBehaviour>();
                for (var j = 0; j < block.amount; j++)
                {
                    if (spawnCount < grid.GetLength())
                    {
                        grid.GetRowCol(spawnCount, out var r, out var c);
                        var passenger = CreatePassenger(block.color, grid[spawnCount++]);
                        passenger.Setup(lane.GetLaneIndex(), r, c);
                        passengers.Add(passenger);
                    }
                } 
                passengerBlocks.Enqueue(new() { color = block.color, passengers = passengers });

                if (spawnCount >= grid.GetLength())
                    break;
            }
        }

        public PassengerBehaviour CreatePassenger(PassengerColor color, Vector3 position)
        {
            var passenger = this.factory.CreatePassenger(color, position);
            passenger.gameObject.name = $"Passenger[{passenger.GetUUID()}]";
            return passenger;
        }
    }
}