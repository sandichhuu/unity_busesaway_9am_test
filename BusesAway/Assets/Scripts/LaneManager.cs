using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public List<Lane> lanes = new List<Lane>();
    
    private void Awake()
    {
        // Initialize 3 lanes
        for (int i = 0; i < 3; i++)
        {
            lanes.Add(new Lane { laneIndex = i });
        }
    }
    
    public void SpawnPassengerInLane(int laneIndex, PassengerColor color)
    {
        if (laneIndex >= 0 && laneIndex < lanes.Count)
        {
            var passenger = new Passenger
            {
                id = Random.Range(1000, 9999),
                Color = color,
                position = Vector3.zero // Starting position
            };
            
            lanes[laneIndex].Enqueue(passenger);
            Debug.Log($"Spawned {color} passenger in lane {laneIndex}");
        }
    }
    
    public List<Passenger> TapLane(int laneIndex)
    {
        if (laneIndex >= 0 && laneIndex < lanes.Count)
        {
            return lanes[laneIndex].MoveToWaitingRoom();
        }
        return new List<Passenger>();
    }
}