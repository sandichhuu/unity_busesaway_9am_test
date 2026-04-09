using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public Lane[] lanes;
    public WaitingRoom waitingRoom;
    
    public void OnLaneTapped(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= lanes.Length) return;
        
        Lane lane = lanes[laneIndex];
        if (!lane.HasPassengers) return;

        //int removed = lane.RemovePassengers(groupSizePerTap);
        //waitingRoom.AddPassengers(removed);
        Debug.Log("LaneTapped");
    }

    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        // Chuyển vị trí touch từ màn hình sang tia Ray
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        // Kiểm tra va chạm (Dùng Raycast cho 3D hoặc Raycast2D cho 2D)
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.transform.parent.TryGetComponent<Lane>(out var lane))
        //            {
        //                Debug.Log($"Touched lane: {lane.gameObject.name}");
        //            }
        //        }
        //    }
        //}

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.TryGetComponent<Lane>(out var lane))
                {
                    Debug.Log($"Touched lane: {lane.gameObject.name}");
                }
            }
        }
    }
}
