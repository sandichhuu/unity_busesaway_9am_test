using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitingRoom
{
    public Dictionary<PassengerColor, List<Passenger>> colorBuckets = new Dictionary<PassengerColor, List<Passenger>>();
    
    public int total => colorBuckets.Values.Sum(l => l.Count);

    public void AddPassengers(PassengerColor color, IEnumerable<Passenger> passengers)
    {
        if (!colorBuckets.ContainsKey(color))
        {
            colorBuckets[color] = new List<Passenger>();
        }
        
        foreach (var passenger in passengers)
        {
            colorBuckets[color].Add(passenger);
        }
    }
    
    public List<Passenger> TakePassengers(PassengerColor color, int maxCount)
    {
        if (!colorBuckets.ContainsKey(color) || colorBuckets[color].Count == 0)
        {
            return new List<Passenger>();
        }
        
        int countToTake = Mathf.Min(maxCount, colorBuckets[color].Count);
        List<Passenger> taken = colorBuckets[color].Take(countToTake).ToList();
        colorBuckets[color].RemoveRange(0, countToTake);
        return taken;
    }
}