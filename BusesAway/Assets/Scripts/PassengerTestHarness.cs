using UnityEngine;
using System.Collections.Generic;

public class PassengerTestHarness : MonoBehaviour
{
    public LaneManager laneManager; // Assign in inspector
    
    private void Start()
    {
        // Create a few passengers of different colors
        List<PassengerColor> colors = new List<PassengerColor>
        {
            PassengerColor.Red,
            PassengerColor.Green,
            PassengerColor.Yellow,
            PassengerColor.Blue,
            PassengerColor.Purple
        };
        
        // Spawn one passenger of each color in lane 0
        for (int i = 0; i < colors.Count; i++)
        {
            laneManager.SpawnPassengerInLane(0, colors[i]);
        }
        
        Debug.Log("Test harness created passengers of all colors");
    }
    
    private void Update()
    {
        // Press space to test tapping lane
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var passengers = laneManager.TapLane(0);
            Debug.Log($"Tapped lane, moved {passengers.Count} passengers to waiting room");
            
            // You would typically pass these to a WaitingRoom here
        }
    }
}