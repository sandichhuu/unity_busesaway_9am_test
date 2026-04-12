using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace BA.Passenger
{
    [Serializable]
    public class PassengerFactory
    {
        [SerializeField, SerializedDictionary("PassengerColor", "Material")] private SerializedDictionary<PassengerColor, Material> passengerMaterials;
        [SerializeField] private PassengerBehaviour passengerPrefab;

        private readonly ObjectPool<PassengerBehaviour> pool;

        public PassengerFactory()
        {
            this.pool = new ObjectPool<PassengerBehaviour>(
                 createFunc: CreateItem,
                 actionOnGet: OnGet,
                 actionOnRelease: OnRelease,
                 actionOnDestroy: OnDestroyItem,
                 collectionCheck: true,
                 defaultCapacity: 100,
                 maxSize: 10000
             );
        }

        public PassengerBehaviour CreatePassenger(PassengerColor color, Vector3 position)
        {
            var passenger = this.pool.Get();
            passenger.Setup(this.pool, color, this.passengerMaterials[color]);
            passenger.transform.position = position;
            return passenger;
        }

        private PassengerBehaviour CreateItem()
        {
            return UnityEngine.Object.Instantiate(this.passengerPrefab);
        }

        private void OnGet(PassengerBehaviour passenger)
        {
            passenger.gameObject.SetActive(true);
        }

        private void OnRelease(PassengerBehaviour passenger)
        {
            passenger.gameObject.SetActive(false);
        }

        private void OnDestroyItem(PassengerBehaviour passenger)
        {
            UnityEngine.Object.Destroy(passenger.gameObject);
        }
    }
}