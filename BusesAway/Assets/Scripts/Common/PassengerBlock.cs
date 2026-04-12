using BA.Passenger;
using System.Collections.Generic;

public class PassengerBlock
{
    public PassengerColor color;
    public List<PassengerBehaviour> passengers;

    public override string ToString()
    {
        return $"{this.color}| {this.passengers.Count}";
    }
}