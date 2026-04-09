using UnityEngine;
using System.Collections.Generic;

public class Route : MonoBehaviour
{
    public string routeName;
    public List<Transform> waypoints = new List<Transform>();
    public Transform pickupStop;
    
    public Vector3 GetWaypointPosition(int index)
    {
        if (index >= 0 && index < waypoints.Count)
            return waypoints[index].position;
        return Vector3.zero;
    }
    
    public int WaypointCount => waypoints.Count;
}
