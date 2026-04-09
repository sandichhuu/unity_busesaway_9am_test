using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector3 offset;
    public int rows = 5;
    public int cols = 6;
    public float spacingX = 0.6f;
    public float spacingY = 0.5f;
    public float rowOffset = 0.3f;
    public List<Vector3> slotPoints = new(128);

    private void Start()
    {
        var pos = this.transform.position;
        var y = pos.y * -1f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                float xOffset = (r % 2 != 0) ? rowOffset : 0f;
                Vector3 newPos = new Vector3(
                    c * spacingX + xOffset,
                    y,
                    r * spacingY
                );

                this.slotPoints.Add(this.transform.position + pos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var pos = this.transform.position;
        var y = pos.y * -1f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                float xOffset = (r % 2 != 0) ? rowOffset : 0f;
                Vector3 newPos = new Vector3(
                    c * spacingX + xOffset,
                    y,
                    r * spacingY
                );

                Gizmos.DrawCube(pos + newPos + this.offset, Vector3.one * 0.1f);
            }
        }
    }
}