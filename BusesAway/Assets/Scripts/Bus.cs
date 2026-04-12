using UnityEngine;
using System.Collections.Generic;

public class Bus : MonoBehaviour
{
    public PassengerColor color;
    public int capacity;
    public List<int> onboardPassengers = new List<int>();
    public bool isFull => this.onboardPassengers.Count >= this.capacity;
    public int availableSeats => this.capacity - this.onboardPassengers.Count;
    
    public bool BoardPassengers(int count)
    {
        int toBoard = Mathf.Min(count, this.availableSeats);
        for (int i = 0; i < toBoard; i++)
        {
            this.onboardPassengers.Add(0);
        }
        return toBoard == count;
    }
    
    public void Clear()
    {
        this.onboardPassengers.Clear();
    }
}
