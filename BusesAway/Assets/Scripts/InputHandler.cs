using UnityEngine;

// Lightweight input handler for selecting buses and moving them via simple input.
public class InputHandler : MonoBehaviour
{
    public static GameObject SelectedBus;

    public static void SelectBus(GameObject bus)
    {
        SelectedBus = bus;
    }

    void Update()
    {
        if (SelectedBus != null)
        {
            // Simple keyboard controls for editor convenience
            if (Input.GetKeyDown(KeyCode.RightArrow)) MovementManager.TryMove(SelectedBus.GetComponent<BusController>(), "Right");
            if (Input.GetKeyDown(KeyCode.LeftArrow)) MovementManager.TryMove(SelectedBus.GetComponent<BusController>(), "Left");
            if (Input.GetKeyDown(KeyCode.UpArrow)) MovementManager.TryMove(SelectedBus.GetComponent<BusController>(), "Up");
            if (Input.GetKeyDown(KeyCode.DownArrow)) MovementManager.TryMove(SelectedBus.GetComponent<BusController>(), "Down");
        }
    }
}
