using UnityEngine;
using System.Collections.Generic;

public class WaitingRoom : MonoBehaviour
{
    public int currentCount;
    public int maxCapacity = 10;
    
    public bool IsFull => currentCount >= maxCapacity;
    public bool HasSpace => currentCount < maxCapacity;
    
    public bool AddPassengers(int count)
    {
        if (!HasSpace) return false;
        int toAdd = Mathf.Min(count, maxCapacity - currentCount);
        currentCount += toAdd;
        return toAdd == count;
    }
    
    public int RemovePassengers(int count)
    {
        int toRemove = Mathf.Min(count, currentCount);
        currentCount -= toRemove;
        return toRemove;
    }
}
