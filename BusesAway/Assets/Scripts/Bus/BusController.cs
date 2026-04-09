using UnityEngine;
using BusesAway.Core;

namespace BusesAway.Bus
{
    public class BusController : MonoBehaviour
    {
        [SerializeField] private string busId;
        [SerializeField] private BusColor busColor;
        [SerializeField] private Direction direction;
        [SerializeField] private Vector2Int gridPosition;

        [Header("Visual")]
        [SerializeField] private MeshRenderer bodyRenderer;
        [SerializeField] private Transform directionIndicator;

        private bool isExiting = false;
        private bool isMoving = false;

        public string BusId => busId;
        public BusColor Color => busColor;
        public Direction Direction => direction;
        public Vector2Int Position
        {
            get => gridPosition;
            set => gridPosition = value;
        }

        public bool IsExiting => isExiting;
        public bool IsMoving => isMoving;

        private void OnValidate()
        {
            UpdateDirectionVisual();
        }

        public void Initialize(string id, BusColor color, Vector2Int pos, Direction dir)
        {
            busId = id;
            busColor = color;
            gridPosition = pos;
            direction = dir;

            UpdateDirectionVisual();
            UpdateColorVisual();
        }

        public void SetMoving(bool moving)
        {
            isMoving = moving;
        }

        public void SetExiting()
        {
            isExiting = true;
        }

        public void UpdatePosition(Vector2Int newPos)
        {
            gridPosition = newPos;
        }

        public bool CanMoveInDirection(Direction moveDirection)
        {
            return moveDirection == direction;
        }

        public Vector2Int GetNextPosition()
        {
            return GetNextPosition(direction);
        }

        public Vector2Int GetNextPosition(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return gridPosition + Vector2Int.up;
                case Direction.Down: return gridPosition + Vector2Int.down;
                case Direction.Left: return gridPosition + Vector2Int.left;
                case Direction.Right: return gridPosition + Vector2Int.right;
                default: return gridPosition;
            }
        }

        private void UpdateDirectionVisual()
        {
            if (directionIndicator == null) return;

            float rotation = 0f;
            switch (direction)
            {
                case Direction.Up: rotation = 0f; break;
                case Direction.Right: rotation = 90f; break;
                case Direction.Down: rotation = 180f; break;
                case Direction.Left: rotation = 270f; break;
            }

            directionIndicator.localRotation = Quaternion.Euler(0, rotation, 0);
        }

        private void UpdateColorVisual()
        {
            if (bodyRenderer == null) return;

            Color color = GetColorForBusColor(busColor);
            bodyRenderer.material.color = color;
        }

        private Color GetColorForBusColor(BusColor bc)
        {
            switch (bc)
            {
                case BusColor.Red: return UnityEngine.Color.red;
                case BusColor.Blue: return UnityEngine.Color.blue;
                case BusColor.Green: return UnityEngine.Color.green;
                case BusColor.Yellow: return UnityEngine.Color.yellow;
                default: return UnityEngine.Color.white;
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            Gizmos.color = GetColorForBusColor(busColor);
            Gizmos.DrawCube(transform.position, Vector3.one * 0.8f);

            Vector3 arrowPos = transform.position;
            Vector3 arrowDir = Vector3.zero;

            switch (direction)
            {
                case Direction.Up: arrowDir = Vector3.forward; break;
                case Direction.Down: arrowDir = Vector3.back; break;
                case Direction.Left: arrowDir = Vector3.left; break;
                case Direction.Right: arrowDir = Vector3.right; break;
            }

            Gizmos.color = UnityEngine.Color.white;
            Gizmos.DrawLine(arrowPos, arrowPos + arrowDir * 0.5f);
        }
    }
}
