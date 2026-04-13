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
    public bool updateRotation = false;
    public List<Vector3> slotPoints = new(128);

    [Header("Flip Settings")]
    public bool flipX = false;
    public bool flipY = false;

    private void Awake()
    {
        CalculatePoints();
    }

    public void CalculatePoints()
    {
        this.slotPoints.Clear();

        // Lưu ý: Y ở đây thường là local offset nếu bạn muốn grid nằm trên mặt phẳng
        // Tôi giữ nguyên logic y = pos.y * -1f của bạn nhưng đưa vào tính toán local
        float localY = 0f;

        for (int r = 0; r < this.rows; r++)
        {
            int actualR = this.flipY ? (this.rows - 1 - r) : r;

            for (int c = 0; c < this.cols; c++)
            {
                int actualC = this.flipX ? (this.cols - 1 - c) : c;

                float xOffset = (actualR % 2 != 0) ? this.rowOffset : 0f;

                Vector3 localPos = new Vector3(
                    actualC * this.spacingX + xOffset,
                    localY,
                    actualR * this.spacingY
                ) + this.offset;

                Vector3 finalPos;
                if (this.updateRotation)
                {
                    finalPos = this.transform.TransformPoint(localPos);
                }
                else
                {
                    finalPos = this.transform.position + localPos;
                }

                this.slotPoints.Add(finalPos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        CalculatePoints();

        var gizmosColor = Gizmos.color;
        for (int i = 0; i < this.slotPoints.Count; i++)
        {
            Gizmos.color = (i == 0) ? Color.red : Color.white;

            if (this.updateRotation)
            {
                // Nếu updateRotation, vẽ Cube theo hướng của Transform
                Gizmos.matrix = Matrix4x4.TRS(this.slotPoints[i], this.transform.rotation, Vector3.one);
                Gizmos.DrawCube(Vector3.zero, Vector3.one * 0.1f);
                Gizmos.matrix = Matrix4x4.identity; // Reset matrix
            }
            else
            {
                Gizmos.DrawCube(this.slotPoints[i], Vector3.one * 0.1f);
            }
        }
        Gizmos.color = gizmosColor;
    }

    public Vector3 this[int index]
    {
        get
        {
            return GetPoint(index);
        }
    }

    public Vector3 GetPoint(int index)
    {
        if (index < 0 || index >= this.slotPoints.Count) return this.transform.position;
        return this.slotPoints[index];
    }

    public Vector3 GetPoint(int r, int c)
    {
        var index = GetIndex(r, c);
        if (index < 0 || index >= this.slotPoints.Count) return this.transform.position;
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

    public void GetRowCol(int index, out int r, out int c)
    {
        if (index < 0 || index >= this.rows * this.cols)
        {
            r = -1;
            c = -1;
            return;
        }

        // Since you add columns in the inner loop:
        r = index / this.cols;
        c = index % this.cols;
    }

    public int GetIndex(int r, int c)
    {
        // Bounds check to prevent out-of-range errors
        if (r < 0 || r >= this.rows || c < 0 || c >= this.cols)
        {
            return -1;
        }

        return (r * this.cols) + c;
    }
}