using UnityEngine;

public enum PassengerColor { Red, Green, Yellow, Blue, Purple }

public class Passenger
{
    public int id;
    public PassengerColor Color;
    public Vector3 position; // for animation, or LocalPosition in a lane queue
}