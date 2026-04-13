using DG.Tweening;
using TMPro;
using UnityEngine;

public class BusStationBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshPro quantityText;

    private Grid grid;
    private int capacity;
    private int amount;

    public Grid GetGrid()
    {
        if (this.grid == null)
            this.grid = GetComponent<Grid>();

        return this.grid;
    }

    public void SetCapacity(int capacity, bool invokePunchScale = false)
    {
        // Cheat
        this.capacity = 180;
        UpdateView(invokePunchScale);
    }

    public int GetCapacity()
    {
        return this.capacity;
    }

    public void SetAmount(int amount, bool invokePunchScale = false)
    {
        this.amount = amount;
        UpdateView(invokePunchScale);
    }

    private void UpdateView(bool invokePunchScale)
    {
        this.quantityText.text = $"{this.amount}/{this.capacity}";
        this.quantityText.color = Color.Lerp(Color.white, Color.red, this.amount * 1.0f / this.capacity);
        if (invokePunchScale)
            this.quantityText.transform.DOPunchScale(Vector3.one * 0.1f, .1f, 1, 1);
    }
}