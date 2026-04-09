using UnityEngine;
using BusesAway.Bus;
using BusesAway.Managers;
using BusesAway.Core;

namespace BusesAway.Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private float swipeThreshold = 50f;
        [SerializeField] private LayerMask busLayer;

        private Camera mainCamera;
        private BusController selectedBus;
        private Vector2 touchStartPos;
        private bool isDragging = false;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                    TrySelectBus();
                    isDragging = true;
                }

                if (touch.phase == TouchPhase.Ended && isDragging)
                {
                    isDragging = false;
                    TryMove();
                    DeselectBus();
                }
            }
            else if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                touchStartPos = UnityEngine.Input.mousePosition;
                TrySelectBus();
                isDragging = true;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
                TryMove();
                DeselectBus();
            }
        }

        private void TrySelectBus()
        {
            Vector2 inputPos = GetInputPosition();
            Ray ray = mainCamera.ScreenPointToRay(inputPos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, busLayer))
            {
                BusController bus = hit.collider.GetComponent<BusController>();
                if (bus != null && !bus.IsExiting)
                {
                    SelectBus(bus);
                }
            }
        }

        private Vector2 GetInputPosition()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                return UnityEngine.Input.GetTouch(0).position;
            }
            return UnityEngine.Input.mousePosition;
        }

        private void SelectBus(BusController bus)
        {
            selectedBus = bus;

            if (MovementManager.Instance != null)
            {
                MovementManager.Instance.SelectBus(bus);
            }
        }

        private void DeselectBus()
        {
            selectedBus = null;

            if (MovementManager.Instance != null)
            {
                MovementManager.Instance.DeselectBus();
            }
        }

        private void TryMove()
        {
            if (selectedBus == null) return;

            Vector2 touchEndPos = GetInputPosition();
            Vector2 swipeDelta = touchEndPos - touchStartPos;

            if (swipeDelta.magnitude < swipeThreshold)
            {
                return;
            }

            Direction? swipeDirection = DetectSwipeDirection(swipeDelta);
            if (swipeDirection.HasValue && MovementManager.Instance != null)
            {
                MovementManager.Instance.TryMoveBus(selectedBus, swipeDirection.Value);
            }
        }

        private Direction? DetectSwipeDirection(Vector2 swipeDelta)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                return swipeDelta.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return swipeDelta.y > 0 ? Direction.Up : Direction.Down;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
