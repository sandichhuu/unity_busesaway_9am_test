using System.Collections.Generic;
using UnityEngine;

public class Bus
{
    public PassengerColor Color;
    public int Capacity;
    public List<Passenger> OnBoard = new List<Passenger>();

    public void AttemptBoard(Passenger p)
    {
        if (p.Color == Color && OnBoard.Count < Capacity)
        {
            OnBoard.Add(p);
        }
    }
    
    public bool IsFull => OnBoard.Count >= Capacity;
}