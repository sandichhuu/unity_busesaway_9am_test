using System.Collections.Generic;
using UnityEngine;

public class Lane
{
    public int laneIndex; // 0,1,2
    public List<Passenger> waitingQueue = new List<Passenger>();
    
    public void Enqueue(Passenger p) 
    { 
        waitingQueue.Add(p); 
    }
    
    public List<Passenger> MoveToWaitingRoom() 
    { 
        var passengers = new List<Passenger>(waitingQueue);
        waitingQueue.Clear();
        return passengers;
    }
}