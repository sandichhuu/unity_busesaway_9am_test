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
                var passengers = new List<PassengerBehaviour>();
                for (var j = 0; j < block.amount; j++)
                {
                    if (spawnCount < grid.GetLength())
                        passengers.Add(CreatePassenger(block.color, grid[spawnCount++]));
                }
                passengerBlocks.Enqueue(new() { color = block.color, passengers = passengers });

                if (spawnCount >= grid.GetLength())
                    break;
            }
        }

        private PassengerBehaviour CreatePassenger(PassengerColor color, Vector3 position)
        {
            var passenger = this.factory.CreatePassenger(color, position);
            passenger.gameObject.name = $"Passenger[{passenger.GetUUID()}]";
            return passenger;
        }
    }
}