using UnityEngine;
using UnityEngine.Pool;

namespace BA.Passenger
{
    public class PassengerBehaviour : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private ObjectPool<PassengerBehaviour> pool;

        [SerializeField] private PassengerColor passengerColor;

        public void Setup(ObjectPool<PassengerBehaviour> pool, PassengerColor passengerColor, Material material, Vector3 initialPosition)
        {
            this.pool = pool;
            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
            this.transform.position = initialPosition;
        }

        public PassengerColor GetColor()
        {
            return this.passengerColor;
        }

        public void Release()
        {
            this.pool.Release(this);
        }
    }
}