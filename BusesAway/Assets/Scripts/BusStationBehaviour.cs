using UnityEngine;

public class BusStationBehaviour : MonoBehaviour
{
    private Grid grid;
    private int capacity;

    public Grid GetGrid()
    {
        if (this.grid == null)
            this.grid = GetComponent<Grid>();

        return this.grid; 
    }

    public void SetCapacity(int capacity)
    {
        this.capacity = capacity;
    }

    public int GetCapacity()
    {
        return this.capacity;
    }
}