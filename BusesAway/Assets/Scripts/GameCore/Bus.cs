using UnityEngine;
using System.Collections.Generic;

public class Bus : MonoBehaviour
{
    public PassengerColor color;
    public int capacity;
    public List<int> onboardPassengers = new List<int>();
    public bool isFull => onboardPassengers.Count >= capacity;
    public int availableSeats => capacity - onboardPassengers.Count;
    
    public bool BoardPassengers(int count)
    {
        int toBoard = Mathf.Min(count, availableSeats);
        for (int i = 0; i < toBoard; i++)
        {
            onboardPassengers.Add(0);
        }
        return toBoard == count;
    }
    
    public void Clear()
    {
        onboardPassengers.Clear();
    }
}
