using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    public GameObject busPrefab; // Assign in inspector
    public Transform spawnPoint; // Assign in inspector
    
    public void SpawnBus(PassengerColor color, int capacity = 5)
    {
        if (busPrefab != null && spawnPoint != null)
        {
            GameObject busObj = Instantiate(busPrefab, spawnPoint.position, Quaternion.identity);
            Bus bus = busObj.GetComponent<Bus>();
            if (bus != null)
            {
                bus.Color = color;
                bus.Capacity = capacity;
                Debug.Log($"Spawned {color} bus with capacity {capacity}");
            }
            else
            {
                Debug.LogError("Bus prefab missing Bus component");
            }
        }
    }
}