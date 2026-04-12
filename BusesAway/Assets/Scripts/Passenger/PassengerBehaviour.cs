using UnityEngine;

namespace BA.Passenger
{
    public class PassengerBehaviour : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        [SerializeField] private PassengerColor passengerColor;

        public void Setup(PassengerColor passengerColor, Material material)
        {
            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshRenderer.materials = new Material[] { material };
            this.passengerColor = passengerColor;
        }

        public PassengerColor GetColor()
        {
            return this.passengerColor;
        }
    }
}