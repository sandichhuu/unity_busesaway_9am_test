using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Lane[] lanes;
    public WaitingRoom waitingRoom;
    public int groupSizePerTap = 3;
    
    void Start()
    {
        Lane[] foundLanes = FindObjectsOfType<Lane>();
        lanes = new Lane[3];
        foreach (var lane in foundLanes)
        {
            if (lane.laneIndex >= 0 && lane.laneIndex < 3)
                lanes[lane.laneIndex] = lane;
        }
        waitingRoom = FindObjectOfType<WaitingRoom>();
    }
    
    public void OnLaneTapped(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= lanes.Length) return;
        
        Lane lane = lanes[laneIndex];
        if (!lane.HasPassengers) return;
        
        int removed = lane.RemovePassengers(groupSizePerTap);
        waitingRoom.AddPassengers(removed);
    }
}
