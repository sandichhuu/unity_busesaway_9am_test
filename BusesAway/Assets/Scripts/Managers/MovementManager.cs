using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BusesAway.Bus;
using BusesAway.Core;
using BusesAway.Grid;

namespace BusesAway.Managers
{
    public class MovementManager : MonoBehaviour
    {
        public static MovementManager Instance { get; private set; }

        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private GridManager gridManager;
        private BusController selectedBus;
        private bool isAnimating = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        public void SelectBus(BusController bus)
        {
            selectedBus = bus;
            HighlightBus(bus);
        }

        public void DeselectBus()
        {
            if (selectedBus != null)
            {
                UnhighlightBus(selectedBus);
            }
            selectedBus = null;
        }

        public bool TryMoveBus(BusController bus, Direction direction)
        {
            if (isAnimating) return false;
            if (!bus.CanMoveInDirection(direction)) return false;
            if (!CanMove(bus, direction)) return false;

            Vector2Int nextPos = bus.GetNextPosition(direction);
            StartCoroutine(MoveBusCoroutine(bus, nextPos));
            return true;
        }

        public bool CanMove(BusController bus, Direction direction)
        {
            if (!bus.CanMoveInDirection(direction))
                return false;

            Vector2Int nextPos = bus.GetNextPosition(direction);

            if (gridManager == null || !gridManager.IsInsideGrid(nextPos))
                return false;

            GridCell cell = gridManager.GetCell(nextPos);
            if (cell == null)
                return false;

            if (cell.Type == TileType.Empty || cell.Type == TileType.Exit)
                return true;

            return false;
        }

        public bool HasValidMove(BusController bus)
        {
            return CanMove(bus, bus.Direction);
        }

        private IEnumerator MoveBusCoroutine(BusController bus, Vector2Int targetPos)
        {
            isAnimating = true;
            bus.SetMoving(true);

            Vector3 startWorldPos = bus.transform.position;
            Vector3 targetWorldPos = gridManager.GridToWorld(targetPos) + Vector3.up * 0.5f;

            UpdateGrid(bus.Position, targetPos, bus);

            float elapsed = 0f;
            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = moveCurve.Evaluate(elapsed / moveDuration);
                bus.transform.position = Vector3.Lerp(startWorldPos, targetWorldPos, t);
                yield return null;
            }

            bus.transform.position = targetWorldPos;
            bus.UpdatePosition(targetPos);
            bus.SetMoving(false);

            CheckExit(bus, targetPos);

            isAnimating = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoveCompleted();
            }
        }

        private void UpdateGrid(Vector2Int oldPos, Vector2Int newPos, BusController bus)
        {
            GridCell oldCell = gridManager.GetCell(oldPos);
            if (oldCell != null)
            {
                oldCell.Type = TileType.Empty;
                oldCell.OccupiedBus = null;
            }

            GridCell newCell = gridManager.GetCell(newPos);
            if (newCell != null)
            {
                newCell.OccupiedBus = bus;
                if (newCell.Type != TileType.Exit)
                {
                    newCell.Type = TileType.Bus;
                }
            }
        }

        private void CheckExit(BusController bus, Vector2Int pos)
        {
            GridCell cell = gridManager.GetCell(pos);
            if (cell != null && cell.Type == TileType.Exit && cell.Exit != null)
            {
                if (cell.Exit.RequiredColor == bus.Color)
                {
                    StartCoroutine(ExitBus(bus));
                }
            }
        }

        private IEnumerator ExitBus(BusController bus)
        {
            bus.SetExiting();

            GridCell cell = gridManager.GetCell(bus.Position);
            if (cell != null)
            {
                cell.OccupiedBus = null;
            }

            Vector3 originalScale = bus.transform.localScale;
            float exitDuration = 0.3f;
            float elapsed = 0f;

            while (elapsed < exitDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / exitDuration;
                bus.transform.localScale = originalScale * (1f - t);
                yield return null;
            }

            Destroy(bus.gameObject);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBusExited(bus);
            }
        }

        private void HighlightBus(BusController bus)
        {
            // Could add visual highlight effect here
        }

        private void UnhighlightBus(BusController bus)
        {
            // Could remove visual highlight effect here
        }
    }
}
