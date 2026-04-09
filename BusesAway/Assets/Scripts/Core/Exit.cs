using UnityEngine;

namespace BusesAway.Core
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private BusColor requiredColor;
        [SerializeField] private Vector2Int gridPosition;
        [SerializeField] private MeshRenderer exitRenderer;

        public BusColor RequiredColor => requiredColor;
        public Vector2Int Position
        {
            get => gridPosition;
            set => gridPosition = value;
        }

        public void Initialize(BusColor color, Vector2Int pos)
        {
            requiredColor = color;
            gridPosition = pos;
            UpdateVisual();
        }

        public bool CanExit(BusColor busColor)
        {
            return requiredColor == busColor;
        }

        private void UpdateVisual()
        {
            if (exitRenderer == null) return;

            Color color = GetColorForBusColor(requiredColor);
            exitRenderer.material.color = color * 0.7f;
        }

        private Color GetColorForBusColor(BusColor bc)
        {
            switch (bc)
            {
                case BusColor.Red: return Color.red;
                case BusColor.Blue: return Color.blue;
                case BusColor.Green: return Color.green;
                case BusColor.Yellow: return Color.yellow;
                default: return Color.white;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GetColorForBusColor(requiredColor) * 0.5f;
            Gizmos.DrawCube(transform.position, new Vector3(0.9f, 0.1f, 0.9f));
        }
    }
}
