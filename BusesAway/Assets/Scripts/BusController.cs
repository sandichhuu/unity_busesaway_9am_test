using UnityEngine;

public class BusController : MonoBehaviour
{
    public string busId;
    public string colorName;
    public Vector2Int gridPos;

    public void Move(string dir)
    {
        MovementManager.TryMove(this, dir);
        // Update world position after move (MovementManager handles position change)
        var grid = FindObjectOfType<GridManager>();
        if (grid != null)
        {
            transform.position = grid.GridToWorld(gridPos);
        }
    }

    void OnMouseDown()
    {
        // Select this bus for input handling
        InputHandler.SelectBus(this.gameObject);
        // Simple visual cue
        var r = GetComponent<Renderer>();
        if (r != null) r.material.color = Color.yellow;
    }
}
