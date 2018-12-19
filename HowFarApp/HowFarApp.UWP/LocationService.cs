﻿using Windows.Devices.Geolocation;
using HowFarApp.Views;

namespace HowFarApp.UWP
{
    public class LocationService : ILocationService
    {
        public bool LocationEnabled => new Geolocator().LocationStatus == PositionStatus.Ready;
    }
}