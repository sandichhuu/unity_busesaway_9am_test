using BA.GameStates;
using BA.Lane;
using BA.Level;
using BA.Passenger;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public readonly UpdateSystem updateSystem = new();
    public readonly GameStateManager stateManager = new();
    public readonly LevelManager levelManager = new();

    [SerializeField] private PassengerManager passengerManager;
    [SerializeField] private LaneManager laneManager;

    private void Start()
    {
        this.updateSystem.AddNode(this.passengerManager);
        this.updateSystem.AddNode(this.laneManager);
        this.updateSystem.AddNode(this.stateManager);

        this.levelManager.LoadLevel(1);
        this.stateManager.ChangeState(new LevelStartingState());
    }

    private void Update()
    {
        this.updateSystem.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        this.updateSystem.OnFixedUpdate(Time.fixedDeltaTime);
    }

    public PassengerManager GetPassengerManager()
    {
        return this.passengerManager;
    }

    public LaneManager GetLaneManager()
    {
        return this.laneManager;
    }

    //public Lane[] lanes;
    //public WaitingRoom waitingRoom;

    //public void OnLaneTapped(int laneIndex)
    //{
    //    if (laneIndex < 0 || laneIndex >= lanes.Length) return;

    //    Lane lane = lanes[laneIndex];
    //    if (!lane.HasPassengers) return;

    //    //int removed = lane.RemovePassengers(groupSizePerTap);
    //    //waitingRoom.AddPassengers(removed);
    //    Debug.Log("LaneTapped");
    //}

    //private void Update()
    //{
    //    if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
    //    {
    //        Vector2 screenPos = Pointer.current.position.ReadValue();
    //        Ray ray = Camera.main.ScreenPointToRay(screenPos);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.transform.TryGetComponent<Lane>(out var lane))
    //            {
    //                Debug.Log($"Touched lane: {lane.gameObject.name}");
    //            }
    //        }
    //    }
    //}
}
