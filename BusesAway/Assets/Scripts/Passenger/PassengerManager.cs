using System;
using UnityEngine;

namespace BA.Passenger
{
    [Serializable]
    public class PassengerManager
    {
        [SerializeField] private PassengerFactory factory;

        public PassengerFactory GetFactory()
        {
            return this.factory;
        }
    }
}