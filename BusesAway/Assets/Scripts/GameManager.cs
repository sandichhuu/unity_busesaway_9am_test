using System.Collections.Generic;
using UnityEngine;

// Lightweight orchestrator for the game session.
public class GameManager : MonoBehaviour
{
    private List<BusController> buses = new List<BusController>();

    public void RegisterBus(BusController bus)
    {
        if (bus == null) return;
        if (!buses.Contains(bus)) buses.Add(bus);
    }

    public void StartGame()
    {
        // Placeholder for starting game logic
    }

    public void EndGame()
    {
        // Placeholder for ending game logic
    }
}
