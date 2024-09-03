using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WinPrayersApp.Services;

public class LocationService
{
    public async Task<(double Latitude, double Longitude)> GetLocationAsync()
    {
        var geolocator = new Geolocator();
        var position = await geolocator.GetGeopositionAsync();
        var latitude = position.Coordinate.Point.Position.Latitude;
        var longitude = position.Coordinate.Point.Position.Longitude;

        return (latitude, longitude);
    }
}
