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
    [SerializeField] private BusManager busManager;
    [SerializeField] private LaneManager laneManager;

    private void Start()
    {
        this.updateSystem.AddNode(this.passengerManager);
        this.updateSystem.AddNode(this.laneManager);
        this.updateSystem.AddNode(this.stateManager);

        this.levelManager.LoadLevel(1);
        this.stateManager.ChangeState(new LevelStartingState());

        Application.targetFrameRate = 60;
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

    public BusManager GetBusManager()
    {
        return this.busManager;
    }

    public LaneManager GetLaneManager()
    {
        return this.laneManager;
    }
}
