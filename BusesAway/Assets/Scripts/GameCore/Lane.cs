using UnityEngine;

public enum PassengerColor
{
    Red,
    Green,
    Blue
}

public class Lane : MonoBehaviour
{
    public int laneIndex;
    public PassengerColor color;
    public int passengerCount;
    public int maxPassengers = 20;
    
    public bool HasPassengers => passengerCount > 0;
    public bool IsFull => passengerCount >= maxPassengers;

    public int RemovePassengers(int count)
    {
        int toRemove = Mathf.Min(count, passengerCount);
        passengerCount -= toRemove;
        return toRemove;
    }
}
