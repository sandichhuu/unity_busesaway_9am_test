using BA.Bus;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BusManager
{
    [SerializeField] private BusFactory factory;

    private List<BusBehaviour> buses = new();

    private BusBehaviour CreateBus(PassengerColor color, Vector3 position)
    {
        var bus = this.factory.CreateBus(color, position);
        bus.gameObject.name = $"Bus[{bus.GetUUID()}]";
        this.buses.Add(bus);
        return bus;
    }
}