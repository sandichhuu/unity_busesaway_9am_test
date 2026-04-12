using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace BA.Bus
{
    [Serializable]
    public class BusFactory
    {
        [SerializeField, SerializedDictionary("BusColor", "Material")] private SerializedDictionary<PassengerColor, Material> busMaterials;
        [SerializeField] private BusBehaviour busPrefab;

        private readonly ObjectPool<BusBehaviour> pool;

        public BusFactory()
        {
            this.pool = new ObjectPool<BusBehaviour>(
                 createFunc: CreateItem,
                 actionOnGet: OnGet,
                 actionOnRelease: OnRelease,
                 actionOnDestroy: OnDestroyItem,
                 collectionCheck: true,
                 defaultCapacity: 4,
                 maxSize: 10000
             );
        }

        public BusBehaviour CreateBus(PassengerColor color, Vector3 position)
        {
            var bus = this.pool.Get();
            bus.Setup(this.pool, color, this.busMaterials[color], position);
            return bus;
        }

        private BusBehaviour CreateItem()
        {
            return UnityEngine.Object.Instantiate(this.busPrefab);
        }

        private void OnGet(BusBehaviour bus)
        {
            bus.gameObject.SetActive(true);
        }

        private void OnRelease(BusBehaviour bus)
        {
            bus.gameObject.SetActive(false);
        }

        private void OnDestroyItem(BusBehaviour bus)
        {
            UnityEngine.Object.Destroy(bus.gameObject);
        }
    }
}