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

    [Header("Flip Settings")]
    public bool flipX = false;
    public bool flipY = false;

    private void Awake()
    {
        CalculatePoints();
    }

    private void CalculatePoints()
    {
        this.slotPoints.Clear();
        var pos = this.transform.position;
        var y = pos.y * -1f;

        for (int r = 0; r < this.rows; r++)
        {
            int actualR = this.flipY ? (this.rows - 1 - r) : r;

            for (int c = 0; c < this.cols; c++)
            {
                int actualC = this.flipX ? (this.cols - 1 - c) : c;

                float xOffset = (actualR % 2 != 0) ? this.rowOffset : 0f;
                Vector3 newPos = new(
                    actualC * this.spacingX + xOffset,
                    y,
                    actualR * this.spacingY
                );

                this.slotPoints.Add(pos + newPos + this.offset);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var gizmosColor = Gizmos.color;

        var pos = this.transform.position;
        var y = pos.y * -1f;

        for (int r = 0; r < this.rows; r++)
        {
            int actualR = flipY ? (this.rows - 1 - r) : r;
            for (int c = 0; c < this.cols; c++)
            {
                int actualC = flipX ? (this.cols - 1 - c) : c;

                float xOffset = (actualR % 2 != 0) ? this.rowOffset : 0f;
                Vector3 point = pos + new Vector3(actualC * this.spacingX + xOffset, y, actualR * this.spacingY) + this.offset;

                Gizmos.color = (r == 0 && c == 0) ? Color.red : Color.white;
                Gizmos.DrawCube(point, Vector3.one * 0.1f);
            }
        }

        Gizmos.color = gizmosColor;
    }

    public Vector3 this[int index]
    {
        get
        {
            return this.slotPoints[index];
        }
    }

    public Vector3 GetPoint(int index)
    {
        return this.slotPoints[index];
    }

    public int GetLength()
    {
        return this.slotPoints.Count;
    }

    public List<Vector3> GetShuffled()
    {
        return this.slotPoints.GetShuffled();
    }
}