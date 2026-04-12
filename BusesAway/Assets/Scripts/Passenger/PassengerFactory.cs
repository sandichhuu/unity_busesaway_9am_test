using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace BA.Passenger
{
    [Serializable]
    public class PassengerFactory
    {
        [SerializeField, SerializedDictionary("PassengerColor", "Material")] private SerializedDictionary<PassengerColor, Material> passengerMaterials;
        [SerializeField] private PassengerBehaviour passengerPrefab;

        public PassengerBehaviour CreatePassenger(PassengerColor color, Vector3 position)
        {
            var instance = UnityEngine.Object.Instantiate(this.passengerPrefab, position, Quaternion.identity);
            instance.transform.position = position;
            var passenger = instance.GetComponent<PassengerBehaviour>();
            passenger.Setup(color, this.passengerMaterials[color]);
            return passenger;
        }
    }
}